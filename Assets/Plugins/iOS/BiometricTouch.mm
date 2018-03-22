
#import <UIKit/UIKit.h>
#import <LocalAuthentication/LocalAuthentication.h>


#if UNITY_VERSION <= 434
#import "iPhone_View.h"

#endif



@interface BiometricTouch

- (BOOL)canEvaluatePolicy:(LAPolicy)policy error:(NSError * __autoreleasing *)error;

@end

extern "C" {
    
    void TouchID(const char* gameObjectName) 
    {
        NSLog(@"Game object name is" + gameObjectName);
        
        LAContext *myContext = [[LAContext alloc] init];
        NSError *authError = nil;
        NSString *myLocalizedReasonString = @"Authenticate using your finger";
        if ([myContext canEvaluatePolicy:LAPolicyDeviceOwnerAuthenticationWithBiometrics error:&authError]) {
            
            [myContext evaluatePolicy:LAPolicyDeviceOwnerAuthenticationWithBiometrics
                      localizedReason:myLocalizedReasonString
                                reply:^(BOOL succes, NSError *error) {
                                    
                                    if (succes) {
                                        UnitySendMessage(gameObjectName, "OnTouchAuthResponse", "User is authenticated successfully");
                                        NSLog(@"User is authenticated successfully");
                                    } else {
                                        
                                        switch (error.code) {
                                            case LAErrorAuthenticationFailed:
                                                NSLog(@"Authentication Failed");
                                                break;
                                                
                                            case LAErrorUserCancel:
                                                NSLog(@"User pressed Cancel button");
                                                break;
                                                
                                            case LAErrorUserFallback:
                                                NSLog(@"User pressed \"Enter Password\"");
                                                break;
                                                
                                            default:
                                                NSLog(@"Touch ID is not configured");
                                                break;
                                        }
                                        
                                        NSLog(@"Authentication Fails");
                                    }
                                }];
        } else {
            NSLog(@"Can not evaluate Touch ID");
        }
      
    }
    
    
    
}


