

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import <MessageUI/MessageUI.h>

@interface DocumentHandler : NSObject <UIDocumentInteractionControllerDelegate, MFMailComposeViewControllerDelegate>
{
    NSURL * fileURL;
}

- (id)initWithURL:(NSURL*)unityURL;

- (void)UpdateURL:(NSURL*)unityURL;

- (bool)OpenDocument;

- (UIDocumentInteractionController*)controller:(UIDocumentInteractionController*)controller;

- (void)mailComposeController:(MFMailComposeViewController*)controller didFinishWithResult:(MFMailComposeResult)result error:(nullable NSError *)error;

-(MFMailComposeViewController*)mailController:(MFMailComposeViewController*)mailController;




@end


@implementation DocumentHandler

- (id)initWithURL:(NSURL*)unityURL
{
    self = [super init];
    fileURL = unityURL;
    return self;
}

- (void)UpdateURL:(NSURL*)unityURL {
    fileURL = unityURL;
}

-(UIImage*)imageWithImage:(UIImage*)image scaledToSize:(CGSize)newSize {
    UIGraphicsBeginImageContext(newSize);
    [image drawInRect:CGRectMake(0, 0, newSize.width, newSize.height)];
    UIImage *newImage = UIGraphicsGetImageFromCurrentImageContext();
    UIGraphicsEndImageContext();
    return newImage;
}

- (UIViewController *) documentInteractionControllerViewControllerForPreview: (UIDocumentInteractionController *) controller {
    return UnityGetGLViewController();
}

- (void)prepareForPopoverPresentation:(UIPopoverPresentationController *)popoverPresentationController;
{
    
}

- (void)OpenIt {
    
    UIDocumentInteractionController *interactionController =
    [UIDocumentInteractionController interactionControllerWithURL: fileURL];
    
    // Configure Document Interaction Controller
    [interactionController setDelegate:self];
    
    // [interactionController presentPreviewAnimated:YES];
    UIView *view = [[UIView alloc] init];
    
    NSArray *items = [NSArray arrayWithObjects:fileURL, nil];
    
    
    UIActivityViewController *controller = [[UIActivityViewController alloc]initWithActivityItems:items applicationActivities:nil];
    
    controller.excludedActivityTypes = @[UIActivityTypePrint, UIActivityTypeCopyToPasteboard, UIActivityTypeAssignToContact, UIActivityTypeSaveToCameraRoll, UIActivityTypeAirDrop];
    
    dispatch_async(dispatch_get_main_queue(), ^{
        
        
        // UIDocumentInteractionController *controller = [[UIDocumentInteractionController alloc] init];
        
        
        // if CGRectZero doesn't work, then try view.bounds
        UIViewController *rootViewController = UnityGetGLViewController();
        
        if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad)
        {
         
            controller.popoverPresentationController.sourceView = controller.view;
            controller.popoverPresentationController.sourceRect = controller.view.frame;
            NSLog(@"I AM SETTING THE VIEW MOTHERFUCKER");
            UIPopoverPresentationController *popup = [[UIPopoverPresentationController alloc] initWithPresentedViewController:rootViewController presentingViewController:rootViewController];

        }
        if (controller != nil)
        {
        [[UIApplication sharedApplication].keyWindow.rootViewController presentViewController:controller animated:YES completion:Nil];
        }
    });
    
    NSLog(@"OBEY ME");
    
    
}

- (void)SendAsMail: (NSURL*) url: (NSString*) to: (NSString*) cc: (NSString*) sub {
    
    UIDocumentInteractionController *interactionController =
    [UIDocumentInteractionController interactionControllerWithURL: fileURL];
    
    // Configure Document Interaction Controller
    [interactionController setDelegate:self];
    
    // [interactionController presentPreviewAnimated:YES];
    UIView *view = [[UIView alloc] init];
    
    NSArray *items = [NSArray arrayWithObjects:fileURL, nil];
    
    NSString* t = to;
    
    NSData* pdfDat = [NSData dataWithContentsOfURL:url];
    
    //dispatch_async(dispatch_get_main_queue(), ^{
    
    
        // UIDocumentInteractionController *controller = [[UIDocumentInteractionController alloc] init];
                    NSData* pdf = [NSData dataWithContentsOfURL:url];
    
        // if CGRectZero doesn't work, then try view.bounds
        UIViewController *rootViewController = UnityGetGLViewController();
    
        if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad)
        {
         if ([MFMailComposeViewController canSendMail]) {
            NSArray* mArray = [NSArray arrayWithObjects:to, nil];
            MFMailComposeViewController *mailController = [[MFMailComposeViewController alloc] init];
            
            mailController.mailComposeDelegate = self;
            
            [mailController setToRecipients:mArray];
            [mailController setSubject:sub];
            [mailController setMessageBody:@"Openmind Test" isHTML:NO];
            
            //[mcvc addAttachmentData:datawithcontents mimeType:@"application/pdf" fileName:fileName];

            [mailController addAttachmentData:pdf mimeType:@"application/pdf" fileName:@"OpenMind"];
            
            [[UIApplication sharedApplication].keyWindow.rootViewController presentViewController:mailController animated:YES completion:nil];
         }
        }
    //});
    
    NSLog(@"OBEY ME");
    
    
}
- (void)mailComposeController:(MFMailComposeViewController*)controller didFinishWithResult:(MFMailComposeResult)result error:(NSError*)error {
    [controller dismissViewControllerAnimated:YES completion:nil];
}

@end




static DocumentHandler* docHandler = nil;

// Converts C style string to NSString
NSString* CreateNSString (const char* string)
{
    if (string)
        return [NSString stringWithUTF8String: string];
    else
        return [NSString stringWithUTF8String: ""];
}




extern "C" {
    
    void OpenPDFThenEmail (const char* path, const char* imageOne, const char* imageTwo,
                           const char* emailTo, const char* emailCC, const char* subjectLine)
    {
        NSLog(@"here we will do the same stuff as below but then go on to email it");
        
        NSString* toAddress = CreateNSString(emailTo);
        NSString* ccAddress = CreateNSString(emailCC);
        NSString* subject = CreateNSString(subjectLine);
        
        // Convert path to URL
        NSString * stringPath = CreateNSString(path);
        NSString * imagePathOne = CreateNSString(imageOne);
        NSString * imagePathTwo = CreateNSString(imageTwo);
        
        
        NSURL *urlOne = [NSURL fileURLWithPath:imagePathOne];
        NSURL *urlTwo = [NSURL fileURLWithPath:imagePathTwo];
        
        
        
        NSData * image = [[NSData alloc] initWithContentsOfURL:urlOne];
        NSData * imagetwo = [[NSData alloc]initWithContentsOfURL:urlTwo];
        UIImage* imgOne = [UIImage imageWithData: image];
        UIImage* imgTwo = [UIImage imageWithData:imagetwo];
        
        
        // CONSIDER SETTING ME TO A SUITABLE SIZE, FOR MACS AND OTHER IPADS
        // BECAUSE THIS SETS THE SIZE TO THE IPADS RESOLUTION
        // APPEARS VERY BIG ON COMPUTER
        // LOOK AT ME LATER.
        
        CGFloat width = imgOne.size.width;
        CGFloat height = imgOne.size.height;
        CGSize mSize = CGSizeMake(width, height);
        
        UIImage* trueOne = [docHandler imageWithImage:imgOne scaledToSize:mSize];
        UIImage* trueTwo =[docHandler imageWithImage:imgTwo scaledToSize:mSize];
        
        // got our two pictures as UI imageds nowm which is cool
        // we can create a pdf, add page, add our image to that page
        // then bish bash bosh
        // this may need to be changed later on depending on what the pdf should look like
        // but you should be able to do that on Unity's side, rather than here, which is good.
        
        // NSString *pdfFileName = CreateNSString(unityURL + "Openmind");
        // Create the PDF context using the default page size of 612 x 792.
        
        UIGraphicsBeginPDFContextToFile(stringPath, CGRectZero, nil);
        
        
        NSInteger currentPage = 0;
        
        
        
        
        UIGraphicsBeginPDFPageWithInfo(CGRectMake(0,0,width,height), nil);
        
        [imgOne drawAtPoint:CGPointZero];
        UIGraphicsBeginPDFPageWithInfo(CGRectMake(0,0,width,height), nil);
        [imgTwo drawAtPoint:CGPointZero];
        
        
        UIGraphicsEndPDFContext();
        
        NSURL *unityURL = [NSURL fileURLWithPath:stringPath];
        
        docHandler = [[DocumentHandler alloc] initWithURL:unityURL];
        
        [docHandler UpdateURL:unityURL];
        [docHandler SendAsMail:unityURL :toAddress :ccAddress :subject];
        //[docHandler SendAsMail:unityURL, toAddress, ccAddress, subject];
        
    }
    
    void OpenPDF (const char* path, const char* imageOne, const char* imageTwo)
    {
        
        // Convert path to URL
        NSString * stringPath = CreateNSString(path);
        NSString * imagePathOne = CreateNSString(imageOne);
        NSString * imagePathTwo = CreateNSString(imageTwo);
        
        
        NSURL *urlOne = [NSURL fileURLWithPath:imagePathOne];
        NSURL *urlTwo = [NSURL fileURLWithPath:imagePathTwo];
        
    
        
        NSData * image = [[NSData alloc] initWithContentsOfURL:urlOne];
        NSData * imagetwo = [[NSData alloc]initWithContentsOfURL:urlTwo];
        UIImage* imgOne = [UIImage imageWithData: image];
        UIImage* imgTwo = [UIImage imageWithData:imagetwo];
        

        // CONSIDER SETTING ME TO A SUITABLE SIZE, FOR MACS AND OTHER IPADS
        // BECAUSE THIS SETS THE SIZE TO THE IPADS RESOLUTION
        // APPEARS VERY BIG ON COMPUTER
        // LOOK AT ME LATER.
        
        CGFloat width = imgOne.size.width;
        CGFloat height = imgOne.size.height;
        CGSize mSize = CGSizeMake(width, height);
        
        UIImage* trueOne = [docHandler imageWithImage:imgOne scaledToSize:mSize];
        UIImage* trueTwo =[docHandler imageWithImage:imgTwo scaledToSize:mSize];
        
        // got our two pictures as UI imageds nowm which is cool
        // we can create a pdf, add page, add our image to that page
        // then bish bash bosh
        // this may need to be changed later on depending on what the pdf should look like
        // but you should be able to do that on Unity's side, rather than here, which is good.
        
        // NSString *pdfFileName = CreateNSString(unityURL + "Openmind");
        // Create the PDF context using the default page size of 612 x 792.
        
        UIGraphicsBeginPDFContextToFile(stringPath, CGRectZero, nil);
        
 
        NSInteger currentPage = 0;
        

        
        
        UIGraphicsBeginPDFPageWithInfo(CGRectMake(0,0,width,height), nil);
        
        [imgOne drawAtPoint:CGPointZero];
        UIGraphicsBeginPDFPageWithInfo(CGRectMake(0,0,width,height), nil);
        [imgTwo drawAtPoint:CGPointZero];
        
        
        UIGraphicsEndPDFContext();
        
        NSURL *unityURL = [NSURL fileURLWithPath:stringPath];
        
        docHandler = [[DocumentHandler alloc] initWithURL:unityURL];
        
        [docHandler UpdateURL:unityURL];
        
        [docHandler OpenIt];
        
        
    }
}

