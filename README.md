# MMCS MSConverter
MP3/WAV to ATRAC3plus multi converter (for MMCS Music Server)
<p align="center">
  <span>English</span> |
  <a href="https://github.com/MadLord80/MMCS-MSConverter/blob/master/README.ru.md">Pусский</a>
</p>

Program for converting mp3 and wav music files to ATRAC3Plus format, supported by MMCS.
To convert files to the standard ATRAC3Plus format, the program uses the TraConv.exe console utility
(http://www.vector.co.jp/soft/winnt/art/se492660.html, https://github.com/MadLord80/MMCS-MSConverter/blob/master/TraConv.exe). This utility must be in the same directory where MMCS MSConverter. 

To use the TraConv.exe console utility, you need to install the Sony OpenMG package (Sony Media Library Earth - 
https://github.com/MadLord80/MMCS-MSConverter/blob/master/OpenMG.rar).

Download - https://github.com/MadLord80/MMCS-MSConverter/tree/master/MultiTraConv/bin/Debug

MadLord, 2018 (madlord.info)

Input directory (mp3, wav) - path to the directory with mp3 and/or wav files. The directory can contain any nested directories, the main thing - the presence of files with the extension mp3 and/or wav. Also, each directory must not contain more than 99 files (MMCS limit)!

Output directory (sc) - the path to the directory where you want to save the converted files. Files will be created with the same paths as the original. Files name and extension will change (new extension - *.sc). The name will change to the MMCS format, i.e. NNN - the sequence number of the original file in the corresponding directory.
For example:  
Input directory - d:\music  
First file - d:\music\b2\my rock-n-roll.mp3  
Output directory - d:\sc  
The full path to the converted file will be d:\sc\b2\001.sc

Also in each directory the program will create a TITLE.lst file - it stores descriptions of files in the MMCS format and will be required later for the MMCS Music Server Editor (MSE) program for correct import of files.

Max processes - MMCS MSConverter can convert several files at the same time, but system resources are wasted on this, so there is a limit to not strongly strain the computer. By default, 3.
You can increase the value if you think it's necessary.

Button "Convert" - starts the conversion.
Button "STOP" - stops the conversion. But since if the conversion processes are already running, the program will stop the process when they run out, and not immediately.

