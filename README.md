Langton's Ant
=============

A project I created while experimenting with a "cellular automaton" called [Langton's Ant](https://en.wikipedia.org/wiki/Langton's_ant).

Implemented in F#, based on a version I've done in C++: [langtons-ant](https://github.com/mhadam/langtons-ant/)

The program outputs a PNG file.

The executable takes two arguments:
* a sequence of letters that specify what movement the ant should take (a combination of Rs and Ls, e.g. RLR or LLRR)
* a number of movements (the program should bog down around 10^9 movements)
* a starting orientation for the ant (cardinal directions: N, S, E, W)

Interesting examples
-------------------
A [cardioid](https://en.wikipedia.org/wiki/Cardioid)!

I'd be super interested if there's any formal proof relating this to a Mandelbrot fractal or a _z_ -> _z_<sup>2</sup> complex mapping.

Worth exploring: whether RRLL is a [nephroid](https://en.wikipedia.org/wiki/Nephroid).
```
LangtonsAnt.exe RRLL 500000000 S
```
![cardioid langton's ant](http://i.imgur.com/Myad95a.png)

This one looks cool, I was exploring LR sequences and tried making one with increasing Rs: L-R-L-2R-L-3R-L-4R
```
LangtonsAnt.exe LRLRRLRRRLRRRR 100000000 N
```
![increasing R sequence langton's ant](http://i.imgur.com/pVI5Ewd.png)
