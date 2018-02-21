#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>


@interface DocumentHandler : NSObject <UIDocumentInteractionControllerDelegate>
{
    NSURL * fileURL;
}

- (id)initWithURL:(NSURL*)unityURL;

- (void)UpdateURL:(NSURL*)unityURL;

- (bool)OpenDocument;

- (UIDocumentInteractionController*)controller:(UIDocumentInteractionController*)controller;


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
        [rootViewController presentViewController:controller animated:YES completion:Nil];
    });
    
    NSLog(@"OBEY ME");
    
    
}


- (UIViewController *) documentInteractionControllerViewControllerForPreview: (UIDocumentInteractionController *) controller {
    return UnityGetGLViewController();
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
        // got our two pictures as UI imageds nowm which is cool
        // we can create a pdf, add page, add our image to that page
        // then bish bash bosh
        // this may need to be changed later on depending on what the pdf should look like
        // but you should be able to do that on Unity's side, rather than here, which is good.
        
        // NSString *pdfFileName = CreateNSString(unityURL + "Openmind");
        // Create the PDF context using the default page size of 612 x 792.
        
        UIGraphicsBeginPDFContextToFile(stringPath, CGRectZero, nil);
        
        CGFloat width = [UIScreen mainScreen].bounds.size.width;
        CGFloat height = [UIScreen mainScreen].bounds.size.height;
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

