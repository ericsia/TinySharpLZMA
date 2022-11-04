# 7zip SDK Example prepared by Eric
- This is a runnable console that you can modify yourself
- The sdk is especially useful when you use it for compressing and decompressing a single file.
- The compression ratio to the performance is in the right balance.
<br><br>

## Compiled Size
As of v22.01 the Debug.dll is around 50kb
<br><br>

## Usage
Rename any file to `in.exe` and place it in debug folder then run it

## Update guide
Download the lastest sdk and patch it
<br><br>


## Removed Log
| Folder   | Files(s)                                                      |
|----------|---------------------------------------------------------------|
| Compress | LzmaAlone                                                     |
| Common   | CRC (only need Table), CommandLineParser, InBuffer, OutBuffer |
