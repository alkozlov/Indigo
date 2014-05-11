using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Double.Factorization;
using MathNet.Numerics.LinearAlgebra.Generic;
using MathNet.Numerics.LinearAlgebra.Storage;

namespace Indigo.BusinessLogicLayer.Analysis
{
    public class LsaMatrix : DenseMatrix
    {
        public List<String> MatrixWords { get; private set; }
        public List<DocumentVector> DocumentVectors { get; private set; }

        public LsaMatrix(int rows, int columns, List<String> matrixWords, List<DocumentVector> documentVectors) : base(rows, columns)
        {
            this.DocumentVectors = documentVectors;
            this.MatrixWords = matrixWords;
        }

        public LsaMatrix(Matrix<double> matrix, List<string> matrixWords, List<DocumentVector> documentVectors) : base(matrix)
        {
            this.DocumentVectors = documentVectors;
            this.MatrixWords = matrixWords;
        }

        public static LsaMatrix Create(params DocumentVector[] documentVectors)
        {
            List<String> words = new List<String>();
            foreach (DocumentVector documentVector in documentVectors)
            {
                words.AddRange(documentVector.Select(x => x.Key));
            }

            words = words.Distinct().ToList();

            Matrix<double> matrix = new DenseMatrix(words.Count, documentVectors.Length);

            for (int i = 0; i < words.Count; i++)
            {
                for (int j = 0; j < documentVectors.Length; j++)
                {
                    if (documentVectors[j].ContainsKey(words[i]))
                    {
                        matrix[i, j]++;
                    }
                    else
                    {
                        matrix[i, j] = 0;
                    }
                }
            }

            LsaMatrix lsaMatrix = new LsaMatrix(matrix, words, documentVectors.ToList());
            return lsaMatrix;
        }
    }
}