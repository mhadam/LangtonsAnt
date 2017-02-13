Langton's Ant
=============

A project I created while experimenting with a "cellular automaton" called [Langton's Ant](https://en.wikipedia.org/wiki/Langton's_ant).

Implemented in F#, based on a version I've done in C++: [langtons-ant](https://github.com/mhadam/langtons-ant/)

The program outputs a PNG file.

The executable takes two arguments:
* a sequence of letters that specify what movement the ant should take (eg. RLR or LLRR to name a few)
* a number of movements (the program seems to bog down around 10^9 movements)
* a starting orientation for the ant (cardinal directions: N, S, E, W)

Interesting examples
-------------------
Cardioid!
```
LangtonsAnt.exe RRLL 500000000 S
```
![cardioid langton's ant](http://i.imgur.com/Myad95a.png)
