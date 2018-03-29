#import <Speech/Speech.h>

@interface SpeechRecorderViewController : UIViewController <SFSpeechRecognizerDelegate> {
    SFSpeechRecognizer *speechRecognizer;
    SFSpeechAudioBufferRecognitionRequest *recognitionRequest;
    SFSpeechRecognitionTask *recognitionTask;
    AVAudioEngine *audioEngine;
}
@end
