//
//  Clipboard.m
//  test
//
//  Created by 三百门 on 2017/7/25.
//  Copyright © 2017年 三百门. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "Clipboard.h"

@implementation Clipboard
//将文本复制到IOS剪贴板
- (void)objc_copyTextToClipboard : (NSString*)text
{
    UIPasteboard *pasteboard = [UIPasteboard generalPasteboard];
    pasteboard.string = text;
}
@end

extern "C" {
    static Clipboard *iosClipboard;
    
    void _copyTextToClipboard(const char *textList)
    {
        NSString *text = [NSString stringWithUTF8String: textList] ;
        
        if(iosClipboard == NULL)
        {
            iosClipboard = [[Clipboard alloc] init];
        }
        
        [iosClipboard objc_copyTextToClipboard: text];
    }
    
}
