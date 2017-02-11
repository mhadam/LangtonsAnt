Langton's Ant
=============

A project I created while experimenting with a "cellular automaton" called [Langton's Ant](https://en.wikipedia.org/wiki/Langton's_ant).

Implemented in F#, based on a version I've done in C++: [langtons-ant](https://github.com/mhadam/langtons-ant/)

The program outputs a PNG file.

The executable takes two arguments:
* a sequence of letters that specify what movement the ant should take (eg. RLR or LLRR to name a few)
* a number of movements (the program seems to bog down around 10^9 movements)

Interesting example
-------------------
```
langtons_ant LRRRRRLLR 10000000
```
```
```
output:
![output]
(http://i.imgur.com/5xjaGmR.png)
