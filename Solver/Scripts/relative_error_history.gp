set logscale y
set size ratio 0
plot 'relative_error_history.dat' u 2:3 t 'Relative error history 1' with lines, 'relative_error_history.dat' u 2:4 t 'Relative error history 2' with lines