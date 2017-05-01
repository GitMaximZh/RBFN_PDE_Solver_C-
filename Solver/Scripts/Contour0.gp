set terminal wxt
bind Close "exit gnuplot"
set title 'Network'

set size ratio -1
set xlabel 'x-axis'
set ylabel 'h-axis'

set style data lines
set contour
set dgrid3d
set view map
set cntrparam levels 15
set style data lines

unset surface
unset clabel

splot 'network_graphic0.dat' u 1:2:4 t '' palette