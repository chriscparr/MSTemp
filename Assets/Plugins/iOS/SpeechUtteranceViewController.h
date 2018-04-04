
#import <UIKit/UIKit.h>
#import <AVFoundation/AVFoundation.h>

@interface SpeechUtteranceViewController : UIViewController <AVSpeechSynthesizerDelegate>
{
AVAudioEngine *audioEngine;
}

@end
