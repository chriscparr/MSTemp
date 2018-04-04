#import "SpeechUtteranceViewController.h"
#import "AVFoundation/AVFoundation.h"
#import <Speech/Speech.h>

static NSString * LanguageCode = [NSString stringWithUTF8String:"en-GB"];

@interface SpeechUtteranceViewController () <AVSpeechSynthesizerDelegate>
{
	AVSpeechSynthesizer *speechSynthesizer;
	NSString * speakText;
	
	float pitch;
	float rate;

}
@property (strong, nonatomic) AVSpeechSynthesizer *synthesizerO;

@end



@implementation SpeechUtteranceViewController

- (id)init
{
	self = [super init];	
	speechSynthesizer = [[AVSpeechSynthesizer alloc] init];
    speechSynthesizer.delegate = self;
	return self;
}
- (void)SettingSpeak: (const char *) _language pitchSpeak: (float)_pitch rateSpeak:(float)_rate
{	
    LanguageCode = [NSString stringWithFormat:@"%s", "en-US"];
	pitch = _pitch;
    rate = _rate;
    UnitySendMessage("TextToSpeech", "onMessage", "Setting Success");
    
}
- (void)StartSpeak: (const char *) _text
{

    NSError *error;
    AVAudioSession *audioSession = [AVAudioSession sharedInstance];
    [audioSession setCategory:AVAudioSessionCategoryPlayAndRecord withOptions:AVAudioSessionCategoryOptionDefaultToSpeaker error:&error];
    [audioSession setMode:AVAudioSessionModeDefault error:&error];
    [audioSession setActive:YES error:&error];
    
    NSLog(@"SPEAKING THE LINE");
    speakText = [NSString stringWithUTF8String:_text];
    NSLog(@"SAY :  %@", speakText);
    NSLog(@"WHAT IS OUR CODE    %@",LanguageCode);
    
    AVSpeechUtterance *utterance = [[AVSpeechUtterance alloc] initWithString:speakText];
    utterance.voice = [AVSpeechSynthesisVoice voiceWithLanguage:LanguageCode];
    // utterance.pitchMultiplier = pitch;
    utterance.rate = AVSpeechUtteranceDefaultSpeechRate;
    utterance.volume = 1;
//    utterance.preUtteranceDelay = 0.2f;
//    utterance.postUtteranceDelay = 0.2f;

    [speechSynthesizer speakUtterance:utterance];
}
- (void)StopSpeak
{
    if([speechSynthesizer isSpeaking]) {
        [speechSynthesizer stopSpeakingAtBoundary:AVSpeechBoundaryImmediate];
        AVSpeechUtterance *utterance = [AVSpeechUtterance speechUtteranceWithString:@""];
        [speechSynthesizer speakUtterance:utterance];
        [speechSynthesizer stopSpeakingAtBoundary:AVSpeechBoundaryImmediate];
    }
}

- (void)speechSynthesizer:(AVSpeechSynthesizer *)synthesizer
willSpeakRangeOfSpeechString:(NSRange)characterRange
                utterance:(AVSpeechUtterance *)utterance
{
    NSString *subString = [speakText substringWithRange:characterRange];
    UnitySendMessage("TextToSpeech", "onSpeechRange", [subString UTF8String]);
}

- (void)speechSynthesizer:(AVSpeechSynthesizer *)synthesizer 
didStartSpeechUtterance:(AVSpeechUtterance *)utterance
{
    UnitySendMessage("TextToSpeech", "onStart", "onStart");
}

- (void)speechSynthesizer:(AVSpeechSynthesizer *)synthesizer
 didFinishSpeechUtterance:(AVSpeechUtterance *)utterance
{
    UnitySendMessage("TextToSpeech", "onDone", "onDone");
}

@end

extern "C"{
    SpeechUtteranceViewController *su = [[SpeechUtteranceViewController alloc] init];  
    void _TAG_StartSpeak(const char * _text){
        [su StartSpeak:_text];
    }
	void _TAG_StopSpeak(){
        [su StopSpeak];
    } 
	void _TAG_SettingSpeak(const char * _language, float _pitch, float _rate){
        [su SettingSpeak:_language pitchSpeak:_pitch rateSpeak:_rate];
    }    
}
