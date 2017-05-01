set xrange [-1:2]
set yrange [-1:3]
plot "rbf_settings.dat" u 1:2:3:3 with ellipses lt 2 lc rgb "gray" lw 0.5 title "Width of RBF",  "rbf_settings.dat" u 1:2:($3/3):($3/3):5 with ellipses palette  fs transparent solid 0.3 noborder title "Weight of RBF", "rbf_settings.dat" u 1:2 with points lw 2 pt 7 lc rgb "black" title "Center of RBF"