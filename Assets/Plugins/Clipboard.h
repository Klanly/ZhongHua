//
//  Clipboard.h
//  test
//
//  Created by 三百门 on 2017/7/25.
//  Copyright © 2017年 三百门. All rights reserved.
//

#ifndef Clipboard_h
#define Clipboard_h

@ interface Clipboard : NSObject

extern "C"
{
    /*  compare the namelist with system processes  */
    void _copyTextToClipboard(const char *textList);
}

@end

#endif /* Clipboard_h */
