# MKVhardsub

Little tool to create *hardsubbed* MP4 video from MKV video container.

[![Build Status](https://travis-ci.org/fahminlb33/MKVhardsub.svg?branch=master)](https://travis-ci.org/fahminlb33/MKVhardsub)

This tool is still **work in progress**, use it at your own risk. This tool is made using the power of
**ffmpeg** and **MKVToolNix** with helps from **gMKVExtractGUI**. Licensed under GNU General Public
License Version 3.

This tool can help you to create hardsubbed MP4 video from MKV video. This tool is designed with many
advantages against SubStation Alpha subtitle.

1. Keep subtitle style (fonts, colour, anything).
2. Encode output video to MP4 container using **ffmpeg** x264.
3. Easy to use with embedded subtitle inside MKV or external subtitle file.
4. Supports both x86 and x64 version of Windows.

  This tool is currently reworked in WPF and dropped the WinForms. Be patient for new release!

## How To Use

1. Download *ffmpeg* and *MKVToolNix*, place it on *bin* folder.
```
Place those files here : <ProjectOutput>\bin.
So, the bin folder will contain ffmpeg.exe, mkvmerge.exe and mkvextract.exe.
```

2. Run MKVhardsub from Visual Studio or build it with MSBuild.

3. You should be can use it even without more further tutorial :D

Currently this repo only supports x86 build configuration. But still, you can use
x64 binary ffmpeg for optimized speed.

### Important Note for Debugging and Usage

Always match *ffmpeg* and *MKVToolNix* binary version. If you are running on x86 machine of course you
need all of those binaries in x86 version. But if you are running it in x64 machine, you can use x64
ffmpeg and MKVToolNix binaries even if you use x86 version of MKVhardsub.

From now, I've added settings window so you can change H.264 Preset and CRF value.

## Credit

**ffmpeg** binaries from https://ffmpeg.zeranoe.com/builds/.

**MKVToolNix** binaries from https://mkvtoolnix.download/.

**gMKVExtractGUI** library from https://sourceforge.net/projects/gmkvextractgui/.
