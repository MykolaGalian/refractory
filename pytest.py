
import scipy
import sys
from scipy.optimize import linprog


a1 = sys.argv[1]
a2 = sys.argv[2]
inR = sys.argv[3]
b1 = sys.argv[4]
b2 = sys.argv[5]
outR = sys.argv[6]

a1f=float(a1);
a2f=float(a2);
inRf=float(inR);
b1f=float(b1);
b2f=float(b2);
outRf=float(outR);

# Set up values relating to both minimum and maximum values of y
coefficients_equalities = [[a2f, b2f],[a1f, b1f]]  
constants_equalities = [outRf,inRf]

bounds_x = (1, 1000)  # require 1 <= x <= 1000
bounds_y = (1, 1000)  # require 1 <= y <= 1000

# Find and print the minimal value of y
coefficients_min_y = [0, 1]  
res = linprog(coefficients_min_y,
              A_eq=coefficients_equalities,
              b_eq=constants_equalities,
              bounds=(bounds_x, bounds_y))
print(res.fun)



# Find and print the maximal value of y = minimal value of -x
coefficients_min_x = [1, 0]  
res = linprog(coefficients_min_x,
              A_eq=coefficients_equalities,
              b_eq=constants_equalities,
              bounds=(bounds_x, bounds_y))
print(res.fun) 






 