﻿namespace GenLabs.DnaUtils;

/// <summary>
/// Represents a normalized <see cref="NucleotideCount"/>.
/// </summary>
public readonly struct NormalizedNucleotideCount
{
    private const double EqualityTolerance = .001;

    private readonly double _a;
    private readonly double _c;
    private readonly double _g;
    private readonly double _t;

    /// <summary>
    /// Initializes a new instance of the <see cref="NormalizedNucleotideCount"/> struct.
    /// </summary>
    /// <param name="nucleotideCount">The <see cref="NucleotideCount"/> to normalize.</param>
    /// <param name="factor">The factor to normalize by.</param>
    public NormalizedNucleotideCount(NucleotideCount nucleotideCount, int factor)
    {
        _a = nucleotideCount[Nucleotide.A] / (double) factor;
        _c = nucleotideCount[Nucleotide.C] / (double) factor;
        _g = nucleotideCount[Nucleotide.G] / (double) factor;
        _t = nucleotideCount[Nucleotide.T] / (double) factor;
    }

    /// <summary>
    /// Gets the normalized count of a specific <see cref="Nucleotide"/>.
    /// </summary>
    /// <param name="nucleotide">The <see cref="Nucleotide"/> to get the normalized count for.</param>
    /// <returns>The normalized count of the <see cref="Nucleotide"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when an invalid nucleotide is encountered.</exception>
    public double this[Nucleotide nucleotide] => nucleotide switch
    {
        Nucleotide.A => _a,
        Nucleotide.C => _c,
        Nucleotide.G => _g,
        Nucleotide.T => _t,
        _ => throw new ArgumentOutOfRangeException(nameof(nucleotide))
    };

    /// <summary>
    /// Gets the <see cref="Nucleotide"/>s with the maximum value.
    /// </summary>
    /// <returns>The <see cref="Nucleotide"/>s with the maximum value and their corresponding value.</returns>
    public (Nucleotide Nucleotide, double Value)[] Max()
    {
        var max = new List<(Nucleotide n, double c)> { (Nucleotide.A, _a) };

        foreach (var n in new[] { Nucleotide.C, Nucleotide.G, Nucleotide.T })
        {
            var v = this[n];

            if (Math.Abs(v - max[0].c) < EqualityTolerance)
            {
                max.Add((n, v));
            }
            else if (v > max[0].c)
            {
                max.Clear();
                max.Add((n, v));
            }
        }

        return max.ToArray();
    }

    /// <summary>
    /// Calculates the entropy of the <see cref="NormalizedNucleotideCount"/>.
    /// </summary>
    /// <returns>The entropy of the <see cref="NormalizedNucleotideCount"/>.</returns>
    public double Entropy() => -new[] { _a, _c, _g, _t }
        .Where(v => v > 0)
        .Select(v => v * Math.Log2(v))
        .Sum();
}