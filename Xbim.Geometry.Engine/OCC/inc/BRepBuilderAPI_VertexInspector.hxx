// Created on: 2011-11-24
// Created by: ANNA MASALSKAYA
// Copyright (c) 2011-2014 OPEN CASCADE SAS
//
// This file is part of Open CASCADE Technology software library.
//
// This library is free software; you can redistribute it and/or modify it under
// the terms of the GNU Lesser General Public License version 2.1 as published
// by the Free Software Foundation, with special exception defined in the file
// OCCT_LGPL_EXCEPTION.txt. Consult the file LICENSE_LGPL_21.txt included in OCCT
// distribution for complete text of the license and disclaimer of any warranty.
//
// Alternatively, this file may be used under the terms of Open CASCADE
// commercial license or contractual agreement.

#ifndef _BRepBuilderAPI_VertexInspector_Header
#define _BRepBuilderAPI_VertexInspector_Header

#include <TColStd_ListOfInteger.hxx>
#include <NCollection_Vector.hxx>
#include <gp_XY.hxx>
#include <gp_XYZ.hxx>
#include <NCollection_CellFilter.hxx>

typedef NCollection_Vector<gp_XYZ> VectorOfPoint;

//=======================================================================
//! Class BRepBuilderAPI_VertexInspector 
//!   derived from NCollection_CellFilter_InspectorXYZ
//!   This class define the Inspector interface for CellFilter algorithm, 
//!   working with gp_XYZ points in 3d space.
//!   Used in search of coincidence points with a certain tolerance.
//=======================================================================

class BRepBuilderAPI_VertexInspector : public NCollection_CellFilter_InspectorXYZ
{
public:
  typedef Standard_Integer Target;
  //! Constructor; remembers the tolerance
  BRepBuilderAPI_VertexInspector (const Standard_Real theTol):myTol(theTol*theTol)
  {}

  //! Keep the points used for comparison
  void Add (const gp_XYZ& thePnt)
  {
    myPoints.Append (thePnt);
  }
  
  //! Clear the list of adjacent points
  void ClearResList()
  {
    myResInd.Clear();
  }
  
  //! Set current point to search for coincidence
  void SetCurrent (const gp_XYZ& theCurPnt)
  { 
    myCurrent = theCurPnt;
  }

  //! Get list of indexes of points adjacent with the current
  const TColStd_ListOfInteger& ResInd()
  {
    return myResInd;
  }

  //! Implementation of inspection method
  Standard_EXPORT NCollection_CellFilter_Action Inspect (const Standard_Integer theTarget);

  // //! Returns hash code for this cell, in the range [1, theUpperBound]
  // //! @param theUpperBound the upper bound of the range a computing hash code must be within
  // //! @return a computed hash code, in the range [1, theUpperBound]
  // Standard_Integer HashCode(const Standard_Integer theUpperBound) const
  // {
  //     // // number of bits per each dimension in the hash code
  //     // const std::size_t aDim = index.Size();
  //     // const std::size_t aShiftBits = (BITS(long) - 1) / aDim;
  //     // unsigned int      aCode = 0;
  //     //
  //     // for (std::size_t i = 0; i < aDim; ++i)
  //     // {
  //     //     aCode = (aCode << aShiftBits) ^ index[i];
  //     // }
  //     //
  //     // return ::HashCode(aCode, theUpperBound);
  //     return 0;
  // }

private:
  Standard_Real myTol;
  TColStd_ListOfInteger myResInd;
  VectorOfPoint myPoints;
  gp_XYZ myCurrent;
};

#endif
