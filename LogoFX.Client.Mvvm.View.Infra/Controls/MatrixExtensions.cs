﻿using System.Windows.Media;

namespace LogoFX.Client.Mvvm.View.Infra.Controls
{
    /// <summary>
    /// This set of internal extension methods provide general solutions and 
    /// utilities in a small enough number to not warrant a dedicated extension
    /// methods class.
    /// </summary>
    internal static class MatrixExtensions
    {         
        /// <summary>
        /// Inverts a Matrix. The Invert functionality on the Matrix type is 
        /// internal to the framework only. Since Matrix is a struct, an out 
        /// parameter must be presented.
        /// </summary>
        /// <param name="m">The Matrix object.</param>
        /// <param name="outputMatrix">The matrix to return by an output 
        /// parameter.</param>
        /// <returns>Returns a value indicating whether the type was 
        /// successfully inverted. If the determinant is 0.0, then it cannot 
        /// be inverted and the original instance will remain untouched.</returns>
        public static bool Invert(this Matrix m, out Matrix outputMatrix)
        {
            double determinant = m.M11 * m.M22 - m.M12 * m.M21;
            if (determinant == 0.0)
            {
                outputMatrix = m;
                return false;
            }

            Matrix matCopy = m;
            m.M11 = matCopy.M22 / determinant;
            m.M12 = -1 * matCopy.M12 / determinant;
            m.M21 = -1 * matCopy.M21 / determinant;
            m.M22 = matCopy.M11 / determinant;
            m.OffsetX = (matCopy.OffsetY * matCopy.M21 - matCopy.OffsetX * matCopy.M22) / determinant;
            m.OffsetY = (matCopy.OffsetX * matCopy.M12 - matCopy.OffsetY * matCopy.M11) / determinant;

            outputMatrix = m;
            return true;
        }

    }
}