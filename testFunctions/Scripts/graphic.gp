set terminal wxt
bind Close "exit gnuplot"

set size ratio 0
set xlabel 'x-axis'
set ylabel 'y-axis'

plot 'graphic_data.dat' u 1:2 t 'graphic' with lines