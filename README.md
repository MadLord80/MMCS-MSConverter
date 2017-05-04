# MMCS MultiTraConv
MP3/WAV to ATRAC3plus (WAV) multi converter (for MMCS Music Server)

Program for converting mp3 and wav music files to ATRAC3Plus (WAV) format, supported by MMCS.
To convert files to the standard ATRAC3Plus format, the program uses the TraConv.exe console utility
(http://www.vector.co.jp/soft/winnt/art/se492660.html).  
To use the TraConv.exe console utility, you need to install the Sony OpenMG package (Sony Media Library Earth - 
https://cloud.mail.ru/public/J14i/z8BzvDGqE).

MadLord, 2017 (madlord.info)

Input directory (mp3, wav) - path to the directory with mp3 and/or wav files. The directory can contain any nested directories, the main thing - the presence of files with the extension mp3 and/or wav.

Output directory (oma, sc) - the path to the directory where you want to save the converted files. Files will be created with the same paths. Change only the extension to *.sc (or *.oma).  
For example:  
Input directory - d:\music  
File - d:\music\b2\my rock-n-roll.mp3  
Output directory - d:\sc  
The full path to the converted file will be d:\sc\b2\my rock-n-roll.sc

Max processes - MMCS MultiTraConv can convert several files at the same time, but system resources are wasted on this, so there is a limit to not strongly strain the computer. By default, 1.
You can increase the value if you think it's necessary.

Max bitrate (kb/s) - the bitrate of the converted files. Because did not check the work on MMCS files with a bitrate other than 128 kb/s, then there is no way to change it.

Convert to *.sc - Convert the files directly to the MMCS Music Server format. If the item is not selected, the files will be converted to the standard format ATRAC3plus (*.oma)

Rename to NNN.sc - if the item is selected, when converting to the MMCS Music Server format, the files in each directory will be renamed in MMCS style (ie 001.sc, 002.sc, etc.).

Enable log - write the conversion results to a log file. The name of the log file can be set in the Log-file name parameter.  
The file is created in the same directory as MMCS MultiTraConv.
