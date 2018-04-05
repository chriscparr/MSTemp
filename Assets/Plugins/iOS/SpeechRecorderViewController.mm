//
//  SpeechRecorderViewController.m
//  SpeechToText
//
#import "SpeechRecorderViewController.h"
#import <Speech/Speech.h>

@implementation SpeechRecorderViewController
id thisClass;
static NSString *trueString;
static bool haveComeFromStop = false;

- (id)init
{
    // when this function finishes, just call it again, over and over
    // so you essentially turn this into Siri :-)
    // openmind siri?
    
    thisClass = self;
    
    self = [super init];    
    
    // Initialize the Speech Recognizer with the locale, couldn't find a list of locales
    // but I assume it's standard UTF-8 https://wiki.archlinux.org/index.php/locale
    speechRecognizer = [[SFSpeechRecognizer alloc] initWithLocale:[[NSLocale alloc] initWithLocaleIdentifier:@"en_US"]];
    
    // Set speech recognizer delegate
    speechRecognizer.delegate = self;
    
    // Request the authorization to make sure the user is asked for permission so you can
    // get an authorized response, also remember to change the .plist file, check the repo's
    // readme file or this project's info.plist
    [SFSpeechRecognizer requestAuthorization:^(SFSpeechRecognizerAuthorizationStatus status) {
        switch (status) {
            case SFSpeechRecognizerAuthorizationStatusAuthorized:
                NSLog(@"Authorized");
                break;
            case SFSpeechRecognizerAuthorizationStatusDenied:
                NSLog(@"Denied");
                break;
            case SFSpeechRecognizerAuthorizationStatusNotDetermined:
                NSLog(@"Not Determined");
                break;
            case SFSpeechRecognizerAuthorizationStatusRestricted:
                NSLog(@"Restricted");
                break;
            default:
                break;
        }
    }];
}

- (void)SettingSpeech: (const char *) _language 
{   
    //LanguageCode = [NSString stringWithUTF8String:_language];
    //NSLocale *local =[[NSLocale alloc] initWithLocaleIdentifier:LanguageCode];
    //speechRecognizer = [[SFSpeechRecognizer alloc] initWithLocale:local];
    //UnitySendMessage("SpeechToText", "onMessage", "Setting Success");
}
// recording
- (void)startRecording {
    
    
    // Initialize the AVAudioEngine
    audioEngine = [[AVAudioEngine alloc] init];
    
    // Make sure there's not a recognition task already running
    if (recognitionTask) {
        [recognitionTask cancel];
        recognitionTask = nil;
    }
    
    // Starts an AVAudio Session
    NSError *error;
    AVAudioSession *audioSession = [AVAudioSession sharedInstance];
    [audioSession setCategory:AVAudioSessionCategoryRecord error:&error];
    [audioSession setActive:YES withOptions:AVAudioSessionSetActiveOptionNotifyOthersOnDeactivation error:&error];
    
//    NSTimer *finalTimer;
//
//    finalTimer = [NSTimer scheduledTimerWithTimeInterval:2 target:self selector:@selector(stopRecording) userInfo:nil repeats:NO];
    
    // Starts a recognition process, in the block it logs the input or stops the audio
    // process if there's an error.
    recognitionRequest = [[SFSpeechAudioBufferRecognitionRequest alloc] init];
    AVAudioInputNode *inputNode = audioEngine.inputNode;
    

    
    recognitionRequest.shouldReportPartialResults = YES;
    recognitionTask = [speechRecognizer recognitionTaskWithRequest:recognitionRequest resultHandler:^(SFSpeechRecognitionResult * _Nullable result, NSError * _Nullable error) {
        BOOL isFinal = NO;
        if (result) {
            // Whatever you say in the microphone after pressing the button should be being logged
            // in the console.
            
            NSLog(@"RESULT:%@",result.bestTranscription.formattedString);
            
            NSString *mString = result.bestTranscription.formattedString;
            trueString = mString;

            isFinal = !result.isFinal;
            
            //[finalTimer invalidate];
            
            NSTimer *finalTimer;
            
            finalTimer = [NSTimer scheduledTimerWithTimeInterval:2 target:self selector:@selector(stopRecording) userInfo:nil repeats:NO];
            
            // this works, but calls the stop recording 3 times for whatever fucking reason?
            // INTERESTING.
            // im so fucking lost in my code but it all seems to work?
            
        }
        if (error && haveComeFromStop != true) {
            // right, understand. because you call STOP RECORDING, the bool is set to true
            // and it's NEVER unset back to FALSE. so of course you will never see this?
            NSLog(@"WE HAVE TIMED OUT, SO NOW WE SHOULD REPEAT");
            NSLog(@"%@",[error localizedDescription]);
            
            
            // new line - this doesnt seem to matter?
            // isFinal = YES;
            
            
            
            
//            if (audioEngine.isRunning)
//            {
//            [audioEngine stop];
//            [inputNode removeTapOnBus:0];
//            recognitionRequest = nil;
//            recognitionTask = nil;
//            }
            //NSLog(@"Repeating this function, apparently");
            [self startRecording];
        }
    }];
    
    // Sets the recording format
    AVAudioFormat *recordingFormat = [inputNode outputFormatForBus:0];
    [inputNode installTapOnBus:0 bufferSize:1024 format:recordingFormat block:^(AVAudioPCMBuffer * _Nonnull buffer, AVAudioTime * _Nonnull when) {
        [recognitionRequest appendAudioPCMBuffer:buffer];
    }];
    
    // Starts the audio engine, i.e. it starts listening.
    [audioEngine prepare];
    [audioEngine startAndReturnError:&error];
    NSLog(@"Say Something, I'm listening");
}


- (void)stopRecording {
    NSLog(@"WE HAVE EITHER TIMED OUT, OR MANUALLY CALLED STOP");
    haveComeFromStop = true;
    if (audioEngine.isRunning) {
        {
            if (trueString.length == 0)
            {
                trueString = @"nil";
            }
                UnitySendMessage("SpeechToText", "onResults", [trueString UTF8String]);
                NSLog(@"STOPRECORDING RESULT: %@", trueString);
            
        }
        trueString = @"";
        // [inputNode removeTapOnBus:0];
        [audioEngine stop];
        recognitionTask = nil;
        recognitionRequest = nil;
        [recognitionRequest endAudio];
        NSLog(@"Now THAT WE HAVE OUTPUTTED TO UNITY loop the function");
        //[self startRecording];
        //

    }
    else
    {
        NSLog(@"Now loop the function");
        //[self startRecording];
    }
    // haveComeFromStop = false;
}

- (void)manuallyStopRecording {
    NSLog(@"MANUALLY STOPPED RECORDING, YOU MUST CALL STARTRECORDING TO RECORD AGAIN");

    if (audioEngine.isRunning) {

        trueString = @"";
        // [inputNode removeTapOnBus:0];
        [audioEngine stop];
        recognitionTask = nil;
        recognitionRequest = nil;
        [recognitionRequest endAudio];
        
    }
}


#pragma mark - SFSpeechRecognizerDelegate Delegate Methods

- (void)speechRecognizer:(SFSpeechRecognizer *)speechRecognizer availabilityDidChange:(BOOL)available {
    NSLog(@"Availability:%d",available);
}

@end
extern "C"{
    SpeechRecorderViewController *vc = [[SpeechRecorderViewController alloc] init];
    void _TAG_startRecording(){
        [thisClass startRecording];
    }    
    void _TAG_stopRecording(){
        [thisClass stopRecording];
    }  
    void _TAG_SettingSpeech(const char * _language){
        //[vc SettingSpeech:_language];
    }
    void _TAG_manuallyStopRecording(){
        [thisClass manuallyStopRecording];
    }
}
