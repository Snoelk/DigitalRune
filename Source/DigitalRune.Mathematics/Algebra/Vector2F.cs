// DigitalRune Engine - Copyright (C) DigitalRune GmbH
// This file is subject to the terms and conditions defined in
// file 'LICENSE.TXT', which is part of this source code package.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
#if !NETFX_CORE && !PORTABLE
using DigitalRune.Mathematics.Algebra.Design;
#endif
#if XNA || MONOGAME
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
#endif


namespace DigitalRune.Mathematics.Algebra
{
  /// <summary>
  /// Defines a 2-dimensional vector (single-precision).
  /// </summary>
  /// <remarks>
  /// The two components (x, y) are stored with single-precision.
  /// </remarks>
#if !NETFX_CORE && !SILVERLIGHT && !WP7 && !WP8 && !XBOX && !UNITY && !PORTABLE
  [Serializable]
#endif
#if !NETFX_CORE && !PORTABLE
  [TypeConverter(typeof(Vector2FConverter))]
#endif
#if !XBOX && !UNITY
  [DataContract]
#endif
  public struct Vector2F : IEquatable<Vector2F>
  {
    //--------------------------------------------------------------
    #region Constants
    //--------------------------------------------------------------

    /// <summary>
    /// Returns a <see cref="Vector2F"/> with all of its components set to zero.
    /// </summary>
    public static readonly Vector2F Zero = new Vector2F(0, 0);

    /// <summary>
    /// Returns a <see cref="Vector2F"/> with all of its components set to one.
    /// </summary>
    public static readonly Vector2F One = new Vector2F(1, 1);

    /// <summary>
    /// Returns the x unit <see cref="Vector2F"/> (1, 0).
    /// </summary>
    public static readonly Vector2F UnitX = new Vector2F(1, 0);

    /// <summary>
    /// Returns the value2 unit <see cref="Vector2F"/> (0, 1).
    /// </summary>
    public static readonly Vector2F UnitY = new Vector2F(0, 1);
    #endregion


    //--------------------------------------------------------------
    #region Fields
    //--------------------------------------------------------------

    /// <summary>
    /// The x component.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
#if !XBOX && !UNITY
    [DataMember]
#endif
    public float X;

    /// <summary>
    /// The y component.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
#if !XBOX && !UNITY
    [DataMember]
#endif
    public float Y;
    #endregion


    //--------------------------------------------------------------
    #region Properties
    //--------------------------------------------------------------

    /// <summary>
    /// Gets or sets the component at the specified index.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <value>The component at <paramref name="index"/>.</value>
    /// <remarks>
    /// The index is zero based: x = vector[0], y = vector[1].
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The <paramref name="index"/> is out of range.
    /// </exception>
    public float this[int index]
    {
      get
      {
        switch (index)
        {
          case 0: return X;
          case 1: return Y;
          default: throw new ArgumentOutOfRangeException("index", "The index is out of range. Allowed values are 0 and 1.");
        }
      }
      set
      {
        switch (index)
        {
          case 0: X = value; break;
          case 1: Y = value; break;
          default: throw new ArgumentOutOfRangeException("index", "The index is out of range. Allowed values are 0 and 1.");
        }
      }
    }


    /// <summary>
    /// Gets a value indicating whether a component of the vector is <see cref="float.NaN"/>.
    /// </summary>
    /// <value>
    /// <see langword="true"/> if a component of the vector is <see cref="float.NaN"/>; otherwise, 
    /// <see langword="false"/>.
    /// </value>
    public bool IsNaN
    {
      get { return Numeric.IsNaN(X) || Numeric.IsNaN(Y); }
    }


    /// <summary>
    /// Returns a value indicating whether this vector is normalized (the length is numerically
    /// equal to 1).
    /// </summary>
    /// <value>
    /// <see langword="true"/> if this <see cref="Vector2F"/> is normalized; otherwise, 
    /// <see langword="false"/>.
    /// </value>
    /// <remarks>
    /// <see cref="IsNumericallyNormalized"/> compares the length of this vector against 1.0 using
    /// the default tolerance value (see <see cref="Numeric.EpsilonF"/>).
    /// </remarks>
    public bool IsNumericallyNormalized
    {
      get { return Numeric.AreEqual(LengthSquared, 1.0f); }
    }


    /// <summary>
    /// Returns a value indicating whether this vector has zero size (the length is numerically
    /// equal to 0).
    /// </summary>
    /// <value>
    /// <see langword="true"/> if this vector is numerically zero; otherwise, 
    /// <see langword="false"/>.
    /// </value>
    /// <remarks>
    /// The length of this vector is compared to 0 using the default tolerance value (see 
    /// <see cref="Numeric.EpsilonF"/>).
    /// </remarks>
    public bool IsNumericallyZero
    {
      get { return Numeric.IsZero(LengthSquared, Numeric.EpsilonFSquared); }
    }


    /// <summary>
    /// Gets or sets the length of this vector.
    /// </summary>
    /// <returns>The length of the this vector.</returns>
    /// <exception cref="MathematicsException">
    /// The vector has a length of 0. The length cannot be changed.
    /// </exception>
    [XmlIgnore]
#if XNA || MONOGAME
    [ContentSerializerIgnore]
#endif
    public float Length
    {
      get
      {
        return (float) Math.Sqrt(X * X + Y * Y);
      }
      set
      {
        float length = Length;
        if (Numeric.IsZero(length))
          throw new MathematicsException("Cannot change length of a vector with length 0.");

        float scale = value / length;
        X *= scale;
        Y *= scale;
      }
    }


    /// <summary>
    /// Returns the squared length of this vector.
    /// </summary>
    /// <returns>The squared length of this vector.</returns>
    public float LengthSquared
    {
      get
      {
        return X * X + Y * Y;
      }
    }


    /// <summary>
    /// Returns the normalized vector.
    /// </summary>
    /// <value>The normalized vector.</value>
    /// <remarks>
    /// The property does not change this instance. To normalize this instance you need to call 
    /// <see cref="Normalize"/>.
    /// </remarks>
    /// <exception cref="DivideByZeroException">
    /// The length of the vector is zero. The quaternion cannot be normalized.
    /// </exception>
    public Vector2F Normalized
    {
      get
      {
        Vector2F v = this;
        v.Normalize();
        return v;
      }
    }


    /// <summary>
    /// Returns an arbitrary normalized <see cref="Vector2F"/> that is orthogonal to this vector.
    /// </summary>
    /// <value>An arbitrary normalized orthogonal <see cref="Vector2F"/>.</value>
    public Vector2F Orthonormal
    {
      get
      {
        Vector2F v;
        v.X = -Y;
        v.Y = X;
        v.Normalize();
        return v;
      }
    }


    /// <summary>
    /// Gets the value of the largest component.
    /// </summary>
    /// <value>The value of the largest component.</value>
    public float LargestComponent
    {
      get
      {
        if (X >= Y)
          return X;

        return Y;
      }
    }


    /// <summary>
    /// Gets the index (zero-based) of the largest component.
    /// </summary>
    /// <value>The index (zero-based) of the largest component.</value>
    /// <remarks>
    /// <para>
    /// This method returns the index of the component (X or Y) which has the largest value. The 
    /// index is zero-based, i.e. the index of X is 0. 
    /// </para>
    /// <para>
    /// If both components are equal, 0 is returned.
    /// </para>
    /// </remarks>
    public int IndexOfLargestComponent
    {
      get
      {
        if (X >= Y)
          return 0;

        return 1;
      }
    }


    /// <summary>
    /// Gets the value of the smallest component.
    /// </summary>
    /// <value>The value of the smallest component.</value>
    public float SmallestComponent
    {
      get
      {
        if (X <= Y)
          return X;

        return Y;
      }
    }


    /// <summary>
    /// Gets the index (zero-based) of the smallest component.
    /// </summary>
    /// <value>The index (zero-based) of the smallest component.</value>
    /// <remarks>
    /// <para>
    /// This method returns the index of the component (X or Y) which has the smallest value. The 
    /// index is zero-based, i.e. the index of X is 0. 
    /// </para>
    /// <para>
    /// If both components are equal, 0 is returned.
    /// </para>
    /// </remarks>
    public int IndexOfSmallestComponent
    {
      get
      {
        if (X <= Y)
          return 0;

        return 1;
      }
    }
    #endregion


    //--------------------------------------------------------------
    #region Creation & Cleanup
    //--------------------------------------------------------------

    /// <overloads>
    /// <summary>
    /// Initializes a new instance of <see cref="Vector2F"/>.
    /// </summary>
    /// </overloads>
    /// 
    /// <summary>
    /// Initializes a new instance of <see cref="Vector2F"/>.
    /// </summary>
    /// <param name="x">Initial value for the x component.</param>
    /// <param name="y">Initial value for the y component.</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
    public Vector2F(float x, float y)
    {
      X = x;
      Y = y;
    }


    /// <summary>
    /// Initializes a new instance of <see cref="Vector2F"/>.
    /// </summary>
    /// <param name="componentValue">The initial value for 2 the vector components.</param>
    /// <remarks>
    /// All components are set to <paramref name="componentValue"/>.
    /// </remarks>
    public Vector2F(float componentValue)
    {
      X = componentValue;
      Y = componentValue;
    }


    /// <summary>
    /// Initializes a new instance of <see cref="Vector2F"/>.
    /// </summary>
    /// <param name="components">Array with the initial values for the components x, and y.</param>
    /// <exception cref="IndexOutOfRangeException">
    /// <paramref name="components"/> has less than 2 elements.
    /// </exception>
    /// <exception cref="NullReferenceException">
    /// <paramref name="components"/> must not be <see langword="null"/>.
    /// </exception>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods")]
    public Vector2F(float[] components)
    {
      X = components[0];
      Y = components[1];
    }


    /// <summary>
    /// Initializes a new instance of the <see cref="Vector2F"/> class.
    /// </summary>
    /// <param name="components">List with the initial values for the components x, and y.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="components"/> has less than 2 elements.
    /// </exception>
    /// <exception cref="NullReferenceException">
    /// <paramref name="components"/> must not be <see langword="null"/>.
    /// </exception>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods")]
    public Vector2F(IList<float> components)
    {
      X = components[0];
      Y = components[1];
    }
    #endregion


    //--------------------------------------------------------------
    #region Interfaces and Overrides
    //--------------------------------------------------------------

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
    public override int GetHashCode()
    {
      // ReSharper disable NonReadonlyFieldInGetHashCode
      unchecked
      {
        int hashCode = X.GetHashCode();
        hashCode = (hashCode * 397) ^ Y.GetHashCode();
        return hashCode;
      }
      // ReSharper restore NonReadonlyFieldInGetHashCode
    }


    /// <overloads>
    /// <summary>
    /// Indicates whether a vector and a another object are equal.
    /// </summary>
    /// </overloads>
    /// 
    /// <summary>
    /// Indicates whether this instance and a specified object are equal.
    /// </summary>
    /// <param name="obj">Another object to compare to.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="obj"/> and this instance are the same type and
    /// represent the same value; otherwise, <see langword="false"/>.
    /// </returns>
    public override bool Equals(object obj)
    {
      return obj is Vector2F && this == (Vector2F)obj;
    }


    #region IEquatable<Vector2F> Members
    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    /// <see langword="true"/> if the current object is equal to the other parameter; otherwise, 
    /// <see langword="false"/>.
    /// </returns>
    public bool Equals(Vector2F other)
    {
      return this == other;
    }
    #endregion


    /// <overloads>
    /// <summary>
    /// Returns the string representation of a vector.
    /// </summary>
    /// </overloads>
    /// 
    /// <summary>
    /// Returns the string representation of this vector.
    /// </summary>
    /// <returns>The string representation of this vector.</returns>
    public override string ToString()
    {
      return ToString(CultureInfo.CurrentCulture);
    }


    /// <summary>
    /// Returns the string representation of this vector using the specified culture-specific format
    /// information.
    /// </summary>
    /// <param name="provider">
    /// An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.
    /// </param>
    /// <returns>The string representation of this vector.</returns>
    public string ToString(IFormatProvider provider)
    {
      return string.Format(provider, "({0}; {1})", X, Y);
    }
    #endregion


    //--------------------------------------------------------------
    #region Overloaded Operators
    //--------------------------------------------------------------

    /// <summary>
    /// Negates a vector.
    /// </summary>
    /// <param name="vector">The vector.</param>
    /// <returns>The negated vector.</returns>
    public static Vector2F operator -(Vector2F vector)
    {
      vector.X = -vector.X;
      vector.Y = -vector.Y;
      return vector;
    }


    /// <summary>
    /// Negates a vector.
    /// </summary>
    /// <param name="vector">The vector.</param>
    /// <returns>The negated vector.</returns>
    public static Vector2F Negate(Vector2F vector)
    {
      vector.X = -vector.X;
      vector.Y = -vector.Y;
      return vector;
    }


    /// <summary>
    /// Adds two vectors.
    /// </summary>
    /// <param name="vector1">The first vector.</param>
    /// <param name="vector2">The second vector.</param>
    /// <returns>The sum of the two vectors.</returns>
    public static Vector2F operator +(Vector2F vector1, Vector2F vector2)
    {
      vector1.X += vector2.X;
      vector1.Y += vector2.Y;
      return vector1;
    }


    /// <summary>
    /// Adds two vectors.
    /// </summary>
    /// <param name="vector1">The first vector.</param>
    /// <param name="vector2">The second vector.</param>
    /// <returns>The sum of the two vectors.</returns>
    public static Vector2F Add(Vector2F vector1, Vector2F vector2)
    {
      vector1.X += vector2.X;
      vector1.Y += vector2.Y;
      return vector1;
    }


    /// <summary>
    /// Subtracts a vector from a vector.
    /// </summary>
    /// <param name="minuend">The first vector (minuend).</param>
    /// <param name="subtrahend">The second vector (subtrahend).</param>
    /// <returns>The difference of the two vectors.</returns>
    public static Vector2F operator -(Vector2F minuend, Vector2F subtrahend)
    {
      minuend.X -= subtrahend.X;
      minuend.Y -= subtrahend.Y;
      return minuend;
    }


    /// <summary>
    /// Subtracts a vector from a vector.
    /// </summary>
    /// <param name="minuend">The first vector (minuend).</param>
    /// <param name="subtrahend">The second vector (subtrahend).</param>
    /// <returns>The difference of the two vectors.</returns>
    public static Vector2F Subtract(Vector2F minuend, Vector2F subtrahend)
    {
      minuend.X -= subtrahend.X;
      minuend.Y -= subtrahend.Y;
      return minuend;
    }



    /// <overloads>
    /// <summary>
    /// Multiplies a vector by a scalar or a vector.
    /// </summary>
    /// </overloads>
    /// 
    /// <summary>
    /// Multiplies a vector by a scalar.
    /// </summary>
    /// <param name="vector">The vector.</param>
    /// <param name="scalar">The scalar.</param>
    /// <returns>The vector with each component multiplied by <paramref name="scalar"/>.</returns>
    public static Vector2F operator *(Vector2F vector, float scalar)
    {
      vector.X *= scalar;
      vector.Y *= scalar;
      return vector;
    }


    /// <summary>
    /// Multiplies a vector by a scalar.
    /// </summary>
    /// <param name="vector">The vector.</param>
    /// <param name="scalar">The scalar.</param>
    /// <returns>The vector with each component multiplied by <paramref name="scalar"/>.</returns>
    public static Vector2F operator *(float scalar, Vector2F vector)
    {
      vector.X *= scalar;
      vector.Y *= scalar;
      return vector;
    }


    /// <overloads>
    /// <summary>
    /// Multiplies a vector by a scalar or a vector.
    /// </summary>
    /// </overloads>
    /// 
    /// <summary>
    /// Multiplies a vector by a scalar.
    /// </summary>
    /// <param name="vector">The vector.</param>
    /// <param name="scalar">The scalar.</param>
    /// <returns>The vector with each component multiplied by <paramref name="scalar"/>.</returns>
    public static Vector2F Multiply(float scalar, Vector2F vector)
    {
      vector.X *= scalar;
      vector.Y *= scalar;
      return vector;
    }


    /// <summary>
    /// Multiplies the components of two vectors by each other.
    /// </summary>
    /// <param name="vector1">The first vector.</param>
    /// <param name="vector2">The second vector.</param>
    /// <returns>The component-wise product of the two vectors.</returns>
    public static Vector2F operator *(Vector2F vector1, Vector2F vector2)
    {
      vector1.X *= vector2.X;
      vector1.Y *= vector2.Y;
      return vector1;
    }


    /// <summary>
    /// Multiplies the components of two vectors by each other.
    /// </summary>
    /// <param name="vector1">The first vector.</param>
    /// <param name="vector2">The second vector.</param>
    /// <returns>The component-wise product of the two vectors.</returns>
    public static Vector2F Multiply(Vector2F vector1, Vector2F vector2)
    {
      vector1.X *= vector2.X;
      vector1.Y *= vector2.Y;
      return vector1;
    }


    /// <overloads>
    /// <summary>
    /// Divides the vector by a scalar or a vector.
    /// </summary>
    /// </overloads>
    /// 
    /// <summary>
    /// Divides a vector by a scalar.
    /// </summary>
    /// <param name="vector">The vector.</param>
    /// <param name="scalar">The scalar.</param>
    /// <returns>The vector with each component divided by <paramref name="scalar"/>.</returns>
    public static Vector2F operator /(Vector2F vector, float scalar)
    {
      float f = 1 / scalar;
      vector.X *= f;
      vector.Y *= f;
      return vector;
    }


    /// <overloads>
    /// <summary>
    /// Divides the vector by a scalar or a vector.
    /// </summary>
    /// </overloads>
    /// 
    /// <summary>
    /// Divides a vector by a scalar.
    /// </summary>
    /// <param name="vector">The vector.</param>
    /// <param name="scalar">The scalar.</param>
    /// <returns>The vector with each component divided by <paramref name="scalar"/>.</returns>
    public static Vector2F Divide(Vector2F vector, float scalar)
    {
      float f = 1 / scalar;
      vector.X *= f;
      vector.Y *= f;
      return vector;
    }


    /// <summary>
    /// Divides the components of a vector by the components of another vector.
    /// </summary>
    /// <param name="dividend">The first vector (dividend).</param>
    /// <param name="divisor">The second vector (divisor).</param>
    /// <returns>The component-wise product of the two vectors.</returns>
    public static Vector2F operator /(Vector2F dividend, Vector2F divisor)
    {
      dividend.X /= divisor.X;
      dividend.Y /= divisor.Y;
      return dividend;
    }


    /// <summary>
    /// Divides the components of a vector by the components of another vector.
    /// </summary>
    /// <param name="dividend">The first vector (dividend).</param>
    /// <param name="divisor">The second vector (divisor).</param>
    /// <returns>The component-wise division of the two vectors.</returns>
    public static Vector2F Divide(Vector2F dividend, Vector2F divisor)
    {
      dividend.X /= divisor.X;
      dividend.Y /= divisor.Y;
      return dividend;
    }


    /// <summary>
    /// Tests if two vectors are equal.
    /// </summary>
    /// <param name="vector1">The first vector.</param>
    /// <param name="vector2">The second vector.</param>
    /// <returns>
    /// <see langword="true"/> if the vectors are equal; otherwise <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// For the test the corresponding components of the vectors are compared.
    /// </remarks>
    public static bool operator ==(Vector2F vector1, Vector2F vector2)
    {
      return vector1.X == vector2.X
             && vector1.Y == vector2.Y;
    }


    /// <summary>
    /// Tests if two vectors are not equal.
    /// </summary>
    /// <param name="vector1">The first vector.</param>
    /// <param name="vector2">The second vector.</param>
    /// <returns>
    /// <see langword="true"/> if the vectors are different; otherwise <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// For the test the corresponding components of the vectors are compared.
    /// </remarks>
    public static bool operator !=(Vector2F vector1, Vector2F vector2)
    {
      return vector1.X != vector2.X
          || vector1.Y != vector2.Y;
    }


    /// <summary>
    /// Tests if each component of a vector is greater than the corresponding component of another
    /// vector.
    /// </summary>
    /// <param name="vector1">The first vector.</param>
    /// <param name="vector2">The second vector.</param>
    /// <returns>
    /// <see langword="true"/> if each component of <paramref name="vector1"/> is greater than its
    /// counterpart in <paramref name="vector2"/>; otherwise, <see langword="false"/>.
    /// </returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
    public static bool operator >(Vector2F vector1, Vector2F vector2)
    {
      return vector1.X > vector2.X
          && vector1.Y > vector2.Y;
    }


    /// <summary>
    /// Tests if each component of a vector is greater or equal than the corresponding component of
    /// another vector.
    /// </summary>
    /// <param name="vector1">The first vector.</param>
    /// <param name="vector2">The second vector.</param>
    /// <returns>
    /// <see langword="true"/> if each component of <paramref name="vector1"/> is greater or equal
    /// than its counterpart in <paramref name="vector2"/>; otherwise, <see langword="false"/>.
    /// </returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
    public static bool operator >=(Vector2F vector1, Vector2F vector2)
    {
      return vector1.X >= vector2.X
          && vector1.Y >= vector2.Y;
    }


    /// <summary>
    /// Tests if each component of a vector is less than the corresponding component of another
    /// vector.
    /// </summary>
    /// <param name="vector1">The first vector.</param>
    /// <param name="vector2">The second vector.</param>
    /// <returns>
    /// <see langword="true"/> if each component of <paramref name="vector1"/> is less than its
    /// counterpart in <paramref name="vector2"/>; otherwise, <see langword="false"/>.
    /// </returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
    public static bool operator <(Vector2F vector1, Vector2F vector2)
    {
      return vector1.X < vector2.X
          && vector1.Y < vector2.Y;
    }


    /// <summary>
    /// Tests if each component of a vector is less or equal than the corresponding component of
    /// another vector.
    /// </summary>
    /// <param name="vector1">The first vector.</param>
    /// <param name="vector2">The second vector.</param>
    /// <returns>
    /// <see langword="true"/> if each component of <paramref name="vector1"/> is less or equal than
    /// its counterpart in <paramref name="vector2"/>; otherwise, <see langword="false"/>.
    /// </returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
    public static bool operator <=(Vector2F vector1, Vector2F vector2)
    {
      return vector1.X <= vector2.X
          && vector1.Y <= vector2.Y;
    }


    /// <overloads>
    /// <summary>
    /// Converts a vector to another data type.
    /// </summary>
    /// </overloads>
    /// 
    /// <summary>
    /// Converts a vector to an array of 2 <see langword="float"/> values.
    /// </summary>
    /// <param name="vector">The vector.</param>
    /// <returns>
    /// The array with 2 <see langword="float"/> values. The order of the elements is: x, y
    /// </returns>
    public static explicit operator float[](Vector2F vector)
    {
      return new[] { vector.X, vector.Y };
    }


    /// <summary>
    /// Converts this vector to an array of 2 <see langword="float"/> values.
    /// </summary>
    /// <returns>
    /// The array with 2 <see langword="float"/> values. The order of the elements is: x, y
    /// </returns>
    public float[] ToArray()
    {
      return (float[]) this;
    }


    /// <summary>
    /// Converts a vector to a list of 2 <see langword="float"/> values.
    /// </summary>
    /// <param name="vector">The vector.</param>
    /// <returns>
    /// The list with 2 <see langword="float"/> values. The order of the elements is: x, y
    /// </returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
    public static explicit operator List<float>(Vector2F vector)
    {
      List<float> result = new List<float>(2) { vector.X, vector.Y };
      return result;
    }


    /// <summary>
    /// Converts this vector to a list of 2 <see langword="float"/> values.
    /// </summary>
    /// <returns>
    /// The list with 2 <see langword="float"/> values. The order of the elements is: x, y
    /// </returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
    public List<float> ToList()
    {
      return (List<float>) this;
    }


    /// <overloads>
    /// <summary>
    /// Performs an implicit conversion from <see cref="Vector2F"/> to another data type.
    /// </summary>
    /// </overloads>
    /// 
    /// <summary>
    /// Performs an implicit conversion from <see cref="Vector2F"/> to <see cref="Vector2D"/>.
    /// </summary>
    /// <param name="vector">The DigitalRune <see cref="Vector2F"/>.</param>
    /// <returns>The result of the conversion.</returns>
    public static implicit operator Vector2D(Vector2F vector)
    {
      return new Vector2D(vector.X, vector.Y);
    }


    /// <summary>
    /// Converts this <see cref="Vector2F"/> to <see cref="Vector2D"/>.
    /// </summary>
    /// <returns>The result of the conversion.</returns>
    public Vector2D ToVector2D()
    {
      return new Vector2D(X, Y);
    }


    /// <summary>
    /// Performs an implicit conversion from <see cref="Vector2F"/> to <see cref="VectorF"/>.
    /// </summary>
    /// <param name="vector">The DigitalRune <see cref="Vector2F"/>.</param>
    /// <returns>The result of the conversion.</returns>
    public static implicit operator VectorF(Vector2F vector)
    {
      VectorF result = new VectorF(2);
      result[0] = vector.X; result[1] = vector.Y;
      return result;
    }


    /// <summary>
    /// Converts this <see cref="Vector2F"/> to <see cref="VectorF"/>.
    /// </summary>
    /// <returns>The result of the conversion.</returns>
    public VectorF ToVectorF()
    {
      return this;
    }


#if XNA || MONOGAME
    /// <summary>
    /// Performs an conversion from <see cref="Vector2"/> (XNA Framework) to <see cref="Vector2F"/> 
    /// (DigitalRune Mathematics).
    /// </summary>
    /// <param name="vector">The <see cref="Vector2"/> (XNA Framework).</param>
    /// <returns>The <see cref="Vector2F"/> (DigitalRune Mathematics).</returns>
    /// <remarks>
    /// This method is available only in the XNA-compatible build of the
    /// DigitalRune.Mathematics.dll.
    /// </remarks>
    public static explicit operator Vector2F(Vector2 vector)
    {
      return new Vector2F(vector.X, vector.Y);
    }


    /// <summary>
    /// Converts this <see cref="Vector2F"/> (DigitalRune Mathematics) to <see cref="Vector2"/> 
    /// (XNA Framework).
    /// </summary>
    /// <param name="vector">The <see cref="Vector2"/> (XNA Framework).</param>
    /// <returns>The <see cref="Vector2F"/> (DigitalRune Mathematics).</returns>
    /// <remarks>
    /// This method is available only in the XNA-compatible build of the
    /// DigitalRune.Mathematics.dll.
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
    public static Vector2F FromXna(Vector2 vector)
    {
      return new Vector2F(vector.X, vector.Y);
    }

    
    /// <summary>
    /// Performs an conversion from <see cref="Vector2F"/> (DigitalRune Mathematics) to 
    /// <see cref="Vector2"/> (XNA Framework).
    /// </summary>
    /// <param name="vector">The <see cref="Vector2F"/> (DigitalRune Mathematics).</param>
    /// <returns>The <see cref="Vector2"/> (XNA Framework).</returns>
    /// <remarks>
    /// This method is available only in the XNA-compatible build of the
    /// DigitalRune.Mathematics.dll.
    /// </remarks>
    public static explicit operator Vector2(Vector2F vector)
    {
      return new Vector2(vector.X, vector.Y);
    }


    /// <summary>
    /// Converts this <see cref="Vector2F"/> (DigitalRune Mathematics) to <see cref="Vector2"/> 
    /// (XNA Framework).
    /// </summary>
    /// <returns>The <see cref="Vector2"/> (XNA Framework).</returns>
    /// <remarks>
    /// This method is available only in the XNA-compatible build of the
    /// DigitalRune.Mathematics.dll.
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
    public Vector2 ToXna()
    {
      return new Vector2(X, Y);
    }
#endif
    #endregion


    //--------------------------------------------------------------
    #region Methods
    //--------------------------------------------------------------

    /// <overloads>
    /// <summary>
    /// Sets each vector component to its absolute value.
    /// </summary>
    /// </overloads>
    /// 
    /// <summary>
    /// Sets each vector component to its absolute value.
    /// </summary>
    public void Absolute()
    {
      X = Math.Abs(X);
      Y = Math.Abs(Y);
    }


    /// <overloads>
    /// <summary>
    /// Clamps the vector components to the range [min, max].
    /// </summary>
    /// </overloads>
    /// 
    /// <summary>
    /// Clamps the vector components to the range [min, max].
    /// </summary>
    /// <param name="min">The min limit.</param>
    /// <param name="max">The max limit.</param>
    /// <remarks>
    /// This operation is carried out per component. Component values less than 
    /// <paramref name="min"/> are set to <paramref name="min"/>. Component values greater than 
    /// <paramref name="max"/> are set to <paramref name="max"/>.
    /// </remarks>
    public void Clamp(float min, float max)
    {
      X = MathHelper.Clamp(X, min, max);
      Y = MathHelper.Clamp(Y, min, max);
    }


    /// <overloads>
    /// <summary>
    /// Clamps near-zero vector components to zero.
    /// </summary>
    /// </overloads>
    /// 
    /// <summary>
    /// Clamps near-zero vector components to zero.
    /// </summary>
    /// <remarks>
    /// Each vector component (X and Y) is compared to zero. If the component is in the interval 
    /// [-<see cref="Numeric.EpsilonF"/>, +<see cref="Numeric.EpsilonF"/>] it is set to zero, 
    /// otherwise it remains unchanged.
    /// </remarks>
    public void ClampToZero()
    {
      X = Numeric.ClampToZero(X);
      Y = Numeric.ClampToZero(Y);
    }


    /// <summary>
    /// Clamps near-zero vector components to zero.
    /// </summary>
    /// <param name="epsilon">The tolerance value.</param>
    /// <remarks>
    /// Each vector component (X and Y) is compared to zero. If the component is in the interval 
    /// [-<paramref name="epsilon"/>, +<paramref name="epsilon"/>] it is set to zero, otherwise it 
    /// remains unchanged.
    /// </remarks>
    public void ClampToZero(float epsilon)
    {
      X = Numeric.ClampToZero(X, epsilon);
      Y = Numeric.ClampToZero(Y, epsilon);
    }


    /// <summary>
    /// Normalizes the vector.
    /// </summary>
    /// <remarks>
    /// A vectors is normalized by dividing its components by the length of the vector.
    /// </remarks>
    /// <exception cref="DivideByZeroException">
    /// The length of this vector is zero. The vector cannot be normalized.
    /// </exception>
    public void Normalize()
    {
      float length = Length;
      if (Numeric.IsZero(length))
        throw new DivideByZeroException("Cannot normalize a vector with length 0.");

      float scale = 1.0f / length;
      X *= scale;
      Y *= scale;
    }


    /// <summary>
    /// Tries to normalize the vector.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if the vector was normalized; otherwise, <see langword="false"/> if 
    /// the vector could not be normalized. (The length is numerically zero.)
    /// </returns>
    public bool TryNormalize()
    {
      float lengthSquared = LengthSquared;
      if (Numeric.IsZero(lengthSquared, Numeric.EpsilonFSquared))
        return false;

      float length = (float)Math.Sqrt(lengthSquared);

      float scale = 1.0f / length;
      X *= scale;
      Y *= scale;

      return true;
    }


    /// <overloads>
    /// <summary>
    /// Projects a vector onto another vector.
    /// </summary>
    /// </overloads>
    /// 
    /// <summary>
    /// Sets this vector to its projection onto the axis given by the target vector.
    /// </summary>
    /// <param name="target">The target vector.</param>
    public void ProjectTo(Vector2F target)
    {
      this = Dot(this, target) / target.LengthSquared * target;
    }
    #endregion


    //--------------------------------------------------------------
    #region Static Methods
    //--------------------------------------------------------------

    /// <summary>
    /// Returns a vector with the absolute values of the elements of the given vector.
    /// </summary>
    /// <param name="vector">The vector.</param>
    /// <returns>A vector with the absolute values of the elements of the given vector.</returns>
    public static Vector2F Absolute(Vector2F vector)
    {
      return new Vector2F(Math.Abs(vector.X), Math.Abs(vector.Y));
    }


    /// <overloads>
    /// <summary>
    /// Determines whether two vectors are equal (regarding a given tolerance).
    /// </summary>
    /// </overloads>
    /// 
    /// <summary>
    /// Determines whether two vectors are equal (regarding the tolerance 
    /// <see cref="Numeric.EpsilonF"/>).
    /// </summary>
    /// <param name="vector1">The first vector.</param>
    /// <param name="vector2">The second vector.</param>
    /// <returns>
    /// <see langword="true"/> if the vectors are equal (within the tolerance 
    /// <see cref="Numeric.EpsilonF"/>); otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// The two vectors are compared component-wise. If the differences of the components are less
    /// than <see cref="Numeric.EpsilonF"/> the vectors are considered as being equal.
    /// </remarks>
    public static bool AreNumericallyEqual(Vector2F vector1, Vector2F vector2)
    {
      return Numeric.AreEqual(vector1.X, vector2.X)
          && Numeric.AreEqual(vector1.Y, vector2.Y);
    }


    /// <summary>
    /// Determines whether two vectors are equal (regarding a specific tolerance).
    /// </summary>
    /// <param name="vector1">The first vector.</param>
    /// <param name="vector2">The second vector.</param>
    /// <param name="epsilon">The tolerance value.</param>
    /// <returns>
    /// <see langword="true"/> if the vectors are equal (within the tolerance 
    /// <paramref name="epsilon"/>); otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// The two vectors are compared component-wise. If the differences of the components are less
    /// than <paramref name="epsilon"/> the vectors are considered as being equal.
    /// </remarks>
    public static bool AreNumericallyEqual(Vector2F vector1, Vector2F vector2, float epsilon)
    {
      return Numeric.AreEqual(vector1.X, vector2.X, epsilon)
          && Numeric.AreEqual(vector1.Y, vector2.Y, epsilon);
    }


    /// <summary>
    /// Returns a vector with the vector components clamped to the range [min, max].
    /// </summary>
    /// <param name="vector">The vector.</param>
    /// <param name="min">The min limit.</param>
    /// <param name="max">The max limit.</param>
    /// <returns>A vector with clamped components.</returns>
    /// <remarks>
    /// This operation is carried out per component. Component values less than 
    /// <paramref name="min"/> are set to <paramref name="min"/>. Component values greater than 
    /// <paramref name="max"/> are set to <paramref name="max"/>.
    /// </remarks>
    public static Vector2F Clamp(Vector2F vector, float min, float max)
    {
      return new Vector2F(MathHelper.Clamp(vector.X, min, max),
                          MathHelper.Clamp(vector.Y, min, max));
    }


    /// <summary>
    /// Returns a vector with near-zero vector components clamped to 0.
    /// </summary>
    /// <param name="vector">The vector.</param>
    /// <returns>The vector with small components clamped to zero.</returns>
    /// <remarks>
    /// Each vector component (X and Y) is compared to zero. If the component is in the interval 
    /// [-<see cref="Numeric.EpsilonF"/>, +<see cref="Numeric.EpsilonF"/>] it is set to zero, 
    /// otherwise it remains unchanged.
    /// </remarks>
    public static Vector2F ClampToZero(Vector2F vector)
    {
      vector.X = Numeric.ClampToZero(vector.X);
      vector.Y = Numeric.ClampToZero(vector.Y);
      return vector;
    }


    /// <summary>
    /// Returns a vector with near-zero vector components clamped to 0.
    /// </summary>
    /// <param name="vector">The vector.</param>
    /// <param name="epsilon">The tolerance value.</param>
    /// <returns>The vector with small components clamped to zero.</returns>
    /// <remarks>
    /// Each vector component (X and Y) is compared to zero. If the component is in the interval 
    /// [-<paramref name="epsilon"/>, +<paramref name="epsilon"/>] it is set to zero, otherwise it 
    /// remains unchanged.
    /// </remarks>
    public static Vector2F ClampToZero(Vector2F vector, float epsilon)
    {
      vector.X = Numeric.ClampToZero(vector.X, epsilon);
      vector.Y = Numeric.ClampToZero(vector.Y, epsilon);
      return vector;
    }


    /// <summary>
    /// Calculates the dot product of two vectors.
    /// </summary>
    /// <param name="vector1">The first vector.</param>
    /// <param name="vector2">The second vector.</param>
    /// <returns>The dot product.</returns>
    /// <remarks>
    /// The method calculates the dot product (also known as scalar product or inner product).
    /// </remarks>
    public static float Dot(Vector2F vector1, Vector2F vector2)
    {
      return vector1.X * vector2.X + vector1.Y * vector2.Y;
    }


    /// <summary>
    /// Calculates the angle between two vectors.
    /// </summary>
    /// <param name="vector1">The first vector.</param>
    /// <param name="vector2">The second vector.</param>
    /// <returns>The angle between the given vectors, such that 0 ≤ angle ≤ π.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="vector1"/> or <paramref name="vector2"/> has a length of 0.
    /// </exception>
    public static float GetAngle(Vector2F vector1, Vector2F vector2)
    {
      if (!vector1.TryNormalize() || !vector2.TryNormalize())
        throw new ArgumentException("vector1 and vector2 must not have 0 length.");

      float α = Dot(vector1, vector2);

      // Inaccuracy in the floating-point operations can cause
      // the result be outside of the valid range.
      // Ensure that the dot product α lies in the interval [-1, 1].
      // Math.Acos() returns Double.NaN if the argument lies outside
      // of this interval.
      α = MathHelper.Clamp(α, -1.0f, 1.0f);

      return (float) Math.Acos(α);
    }


    /// <summary>
    /// Returns a vector that contains the lowest value from each matching pair of components.
    /// </summary>
    /// <param name="vector1">The first vector.</param>
    /// <param name="vector2">The second vector.</param>
    /// <returns>The minimized vector.</returns>
    public static Vector2F Min(Vector2F vector1, Vector2F vector2)
    {
      vector1.X = Math.Min(vector1.X, vector2.X);
      vector1.Y = Math.Min(vector1.Y, vector2.Y);
      return vector1;
    }


    /// <summary>
    /// Returns a vector that contains the highest value from each matching pair of components.
    /// </summary>
    /// <param name="vector1">The first vector.</param>
    /// <param name="vector2">The second vector.</param>
    /// <returns>The maximized vector.</returns>
    public static Vector2F Max(Vector2F vector1, Vector2F vector2)
    {
      vector1.X = Math.Max(vector1.X, vector2.X);
      vector1.Y = Math.Max(vector1.Y, vector2.Y);
      return vector1;
    }


    /// <summary>
    /// Projects a vector onto an axis given by the target vector.
    /// </summary>
    /// <param name="vector">The vector.</param>
    /// <param name="target">The target vector.</param>
    /// <returns>
    /// The projection of <paramref name="vector"/> onto <paramref name="target"/>.
    /// </returns>
    public static Vector2F ProjectTo(Vector2F vector, Vector2F target)
    {
      return Dot(vector, target) / target.LengthSquared * target;
    }


    /// <overloads>
    /// <summary>
    /// Converts the string representation of a 2-dimensional vector to its <see cref="Vector2F"/>
    /// equivalent.
    /// </summary>
    /// </overloads>
    /// 
    /// <summary>
    /// Converts the string representation of a 2-dimensional vector to its <see cref="Vector2F"/>
    /// equivalent.
    /// </summary>
    /// <param name="s">A string representation of a 2-dimensional vector.</param>
    /// <returns>
    /// A <see cref="Vector2F"/> that represents the vector specified by the <paramref name="s"/>
    /// parameter.
    /// </returns>
    /// <remarks>
    /// This version of <see cref="Parse(string)"/> uses the <see cref="CultureInfo"/> associated
    /// with the current thread.
    /// </remarks>
    /// <exception cref="FormatException">
    /// <paramref name="s"/> is not a valid <see cref="Vector2F"/>.
    /// </exception>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
    public static Vector2F Parse(string s)
    {
      return Parse(s, CultureInfo.CurrentCulture);
    }


    /// <summary>
    /// Converts the string representation of a 2-dimensional vector in a specified culture-specific
    /// format to its <see cref="Vector2F"/> equivalent.
    /// </summary>
    /// <param name="s">A string representation of a 2-dimensional vector.</param>
    /// <param name="provider">
    /// An <see cref="IFormatProvider"/> that supplies culture-specific formatting information about
    /// <paramref name="s"/>. 
    /// </param>
    /// <returns>
    /// A <see cref="Vector2F"/> that represents the vector specified by the <paramref name="s"/>
    /// parameter.
    /// </returns>
    /// <exception cref="FormatException">
    /// <paramref name="s"/> is not a valid <see cref="Vector2F"/>.
    /// </exception>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
    public static Vector2F Parse(string s, IFormatProvider provider)
    {
      Match m = Regex.Match(s, @"\((?<x>.*);(?<y>.*)\)", RegexOptions.None);
      if (m.Success)
      {
        return new Vector2F(float.Parse(m.Groups["x"].Value, provider),
          float.Parse(m.Groups["y"].Value, provider));
      }
      
      throw new FormatException("String is not a valid Vector2F.");
    }
    #endregion
  }
}
