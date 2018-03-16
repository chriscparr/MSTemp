
#import <UIKit/UIKit.h>
#import <LocalAuthentication/LocalAuthentication.h>


#if UNITY_VERSION <= 434
#import "iPhone_View.h"

#endif



//@interface LoginView : LAContext
//
//
//@end

//@implementation NonRotatingUIImagePickerController
//- (NSUInteger)supportedInterfaceOrientations{
//    return UIInterfaceOrientationMaskLandscape;
//}



//-----------------------------------------------------------------

//@interface APLViewController : UIViewController <UINavigationControllerDelegate, UIImagePickerControllerDelegate>{
//
//    UIImagePickerController *imagePickerController;
//@public
//    const char *callback_game_object_name ;
//    const char *callback_function_name ;
//}
//
//@end


@interface LoginView

- (BOOL)canEvaluatePolicy:(LAPolicy)policy error:(NSError * __autoreleasing *)error;

@end

extern "C" {
    
    int i = 0;
    
    bool TouchID() {
        
        LAContext *myContext = [[LAContext alloc] init];
        NSError *authError = nil;
        NSString *myLocalizedReasonString = @"Authenticate using your finger";
        if ([myContext canEvaluatePolicy:LAPolicyDeviceOwnerAuthenticationWithBiometrics error:&authError]) {
            
            [myContext evaluatePolicy:LAPolicyDeviceOwnerAuthenticationWithBiometrics
                      localizedReason:myLocalizedReasonString
                                reply:^(BOOL succes, NSError *error) {
                                    
                                    if (succes) {
                                        i = 1;
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
                                        // return false;
                                        i = 0;
                                    }
                                }];
        } else {
            NSLog(@"Can not evaluate Touch ID");
            i = 1;
        }
        if (i == 1)
        {
            UnitySendMessage("LoginView", "Success", "");
            return true;
        }
        else
        {
            return false;
        }
      
    }
    
    
    
}


