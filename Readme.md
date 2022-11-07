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
Download the latest sdk and patch it
<br><br>


## Removed Log
| Folder   | Files(s)                                                      |
|----------|---------------------------------------------------------------|
| Common   | CRC (only need Table), CommandLineParser, InBuffer, OutBuffer |
| Compress | LzmaAlone                                                     |
<br><br>


## speed improvement code:
- `LzBintree.cs` GetMatched and Skip
- basically trying to find the match length a single byte at a time
- compare 8 bytes at a time instead of 1, will improve the speed. Change byte* to a ulong*
```c#
if (_bufferBase[pby1 + len] == _bufferBase[cur + len])
{
    while (++len != lenLimit)
        if (_bufferBase[pby1 + len] != _bufferBase[cur + len])
            break;
```

## redundant code:
- `LzBintree.cs` GetMatched and Skip, BT2 can remove `Common\CRC.cs`
- LzBintree.cs remove all `if (HASH_ARRAY)` if you hardcoded it to use BT2 and vice versa, example
```C#
if (HASH_ARRAY)
{
    UInt32 temp = CRC.Table[_bufferBase[cur]] ^ _bufferBase[cur + 1];
    UInt32 hash2Value = temp & (kHash2Size - 1);
    _hash[hash2Value] = _pos;
    temp ^= ((UInt32)(_bufferBase[cur + 2]) << 8);
    UInt32 hash3Value = temp & (kHash3Size - 1);
    _hash[kHash3Offset + hash3Value] = _pos;
    hashValue = (temp ^ (CRC.Table[_bufferBase[cur + 3]] << 5)) & _hashMask;
}
else
```
