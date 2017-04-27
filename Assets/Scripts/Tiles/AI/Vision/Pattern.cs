using System;
/* Documentation for pattern JSON spec
     Pattern.json must be an array of number arrays
     This is read in as a matrix of floats
     The center square houses the dog
     The orientation of the dog is upwards:
     XXXX    FRONT   XXXX
     LEFT    DOG     RIGHT
     XXXX    BACK    XXXX
     Which works out like this in the JSON matrix (sans comments in parenthesis):
     {
       "probabilities": [
          [0,           0.5 (front), 0],
          [0.25 (left), 1 (dog),     0.25 (right)],
          [0,           0.25(back),  0]
       ]
     }
     The matrix must have odd length sides and be square
     The odd length sides is so their is a center square that houses the dog
     The square requirment is so inversions can be done to orient the vision pattern easily
     If your pattern isn't square, just fill the remaining entries with 0
     If the pattern.json doesn't meet these requirments, an exception will be throw
*/
[Serializable]
public class Pattern
{
	public float[,] probabilities;
}
