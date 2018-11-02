# MMCS MSConverter

Программа для конвертации музыкальных файлов mp3 и wav в формат ATRAC3Plus, поддерживаемый MMCS.
Для конвертации файлов в стандартный формат ATRAC3Plus программа использует консольную утилиту TraConv.exe
(http://www.vector.co.jp/soft/winnt/art/se492660.html, https://github.com/MadLord80/MMCS-MSConverter/blob/master/TraConv.exe). Утилита должна находиться в том же каталоге, где и MMCS MSConverter.

Для работы консольной утилиты TraConv.exe необходимо установить пакет Sony OpenMG (Sony Media Library Earth - 
https://github.com/MadLord80/MMCS-MSConverter/blob/master/OpenMG.rar).

MadLord, 2018 (madlord.info)

Input directory (mp3, wav) - путь к каталогу с mp3 и/или wav файлами. Каталог может содержать любые вложенные 
каталоги, главное - наличие файлов с расширением *.mp3 и/или wav. Также каждый каталог не должен содержать
более 99 файлов (ограничение MMCS)!

Output directory (sc) - путь к каталогу, куда надо сохранить сконвертированные файлы. Файлы будут создаваться
с теми же путями, что и оригинальные. У файлов изменится наименование и расширение (на *.sc).  Наименование 
изменится на формат MMCS, т.е. NNN - порядковый номер оригинального файла в соответствующем каталоге.
Например:
Input directory - d:\music
первый файл - d:\music\russian\b2\мой рок-н-рол.mp3
Output directory - d:\sc
полный путь к сконвертированному файлу будет d:\sc\russian\b2\001.sc

Также в каждом каталоге программа создаст файл TITLE.lst - он хранит описания файлов в формате MMCS
и потребуется в дальнейшем для программы MMCS Music Server Editor (MSE) для корректного импорта
полученных файлов.

Max processes - MMCS MSConverter может конвертировать одновременно несколько файлов, но на это уходят
системные ресурсы, поэтому, чтобы сильно не напрягать комп, стоит ограничение. По уолчанию, 3.
Можете увеличить значение, если считаете нужным.

Кнопка "Convert" - запускает конвертацию.
Кнопка "STOP" - останавливает конвертацию. Но т.к. при этом уже могут происходить запущенные процессы
конвертации, то программа остановит процесс когда они закончатся, а не сразу.