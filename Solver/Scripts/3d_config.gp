set terminal wxt
bind Close "exit gnuplot"

set size ratio -1
set xlabel 'x-axis'
set ylabel 'y-axis'
set format z "%-.1te%+T" 
set format cb "%-.1te%+T"

unset surface
unset hidden3d
set pm3d
set style line 1 linetype 0 lc "#111111"
set pm3d hidden3d 1 interpolate 2, 2
set palette model RGB defined(0 '#333333', 1 '#eeeeee')



