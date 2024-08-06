﻿// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 08-05-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-05-2024
// ******************************************************************************************
// <copyright file="DataRowExtensions.cs" company="Terry D. Eppler">
//    Booger is a quick & dirty WPF application that interacts with OpenAI GPT-3.5 Turbo API
//    based on NET6 and written in C-Sharp.
// 
//    Copyright ©  2024  Terry D. Eppler
// 
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the “Software”),
//    to deal in the Software without restriction,
//    including without limitation the rights to use,
//    copy, modify, merge, publish, distribute, sublicense,
//    and/or sell copies of the Software,
//    and to permit persons to whom the Software is furnished to do so,
//    subject to the following conditions:
// 
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
// 
//    THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
//    INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT.
//    IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
//    DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
//    DEALINGS IN THE SOFTWARE.
// 
//    You can contact me at: terryeppler@gmail.com or eppler.terry@epa.gov
// </copyright>
// <summary>
//   DataRowExtensions.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.OleDb;
    using System.Data.SqlClient;
    using System.Data.SQLite;
    using System.Data.SqlServerCe;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    /// <summary>
    /// 
    /// </summary>
    [ SuppressMessage( "ReSharper", "UseObjectOrCollectionInitializer" ) ]
    [ SuppressMessage( "ReSharper", "ArrangeDefaultValueWhenTypeNotEvident" ) ]
    internal static class DataRowExtensions
    {
        /// <summary>
        /// Converts to sql db parameters.
        /// </summary>
        /// <param name="dataRow">The data row.</param>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        public static IEnumerable<DbParameter> ToSqlDbParameters( this DataRow dataRow,
            Provider provider )
        {
            if( dataRow?.ItemArray.Length > 0
                && Enum.IsDefined( typeof( Provider ), provider ) )
            {
                try
                {
                    {
                        var _table = dataRow.Table;
                        var _columns = _table?.Columns;
                        var _values = dataRow.ItemArray;
                        switch( provider )
                        {
                            case Provider.SQLite:
                            {
                                var _sqlite = new List<SQLiteParameter>( );
                                for( var _i = 0; _i < _columns?.Count; _i++ )
                                {
                                    var _parameter = new SQLiteParameter( );
                                    _parameter.SourceColumn = _columns[ _i ].ColumnName;
                                    _parameter.Value = _values[ _i ];
                                    _sqlite.Add( _parameter );
                                }

                                return _sqlite?.Any( ) == true
                                    ? _sqlite
                                    : default( IList<SQLiteParameter> );
                            }
                            case Provider.SqlCe:
                            {
                                var _sqlce = new List<SqlCeParameter>( );
                                for( var _i = 0; _i < _columns?.Count; _i++ )
                                {
                                    var _parameter = new SqlCeParameter( );
                                    _parameter.SourceColumn = _columns[ _i ].ColumnName;
                                    _parameter.Value = _values[ _i ];
                                    _sqlce.Add( _parameter );
                                }

                                return _sqlce?.Any( ) == true
                                    ? _sqlce
                                    : default( IList<SqlCeParameter> );
                            }
                            case Provider.OleDb:
                            case Provider.Excel:
                            case Provider.Access:
                            {
                                var _oledb = new List<OleDbParameter>( );
                                for( var _i = 0; _i < _columns?.Count; _i++ )
                                {
                                    var _parameter = new OleDbParameter( );
                                    _parameter.SourceColumn = _columns[ _i ].ColumnName;
                                    _parameter.Value = _values[ _i ];
                                    _oledb.Add( _parameter );
                                }

                                return _oledb.Any( )
                                    ? _oledb
                                    : default( IList<OleDbParameter> );
                            }
                            case Provider.SqlServer:
                            {
                                var _sqlserver = new List<SqlParameter>( );
                                for( var _i = 0; _i < _columns?.Count; _i++ )
                                {
                                    var _parameter = new SqlParameter( );
                                    _parameter.SourceColumn = _columns[ _i ].ColumnName;
                                    _parameter.Value = _values[ _i ];
                                    _sqlserver.Add( _parameter );
                                }

                                return _sqlserver?.Any( ) == true
                                    ? _sqlserver
                                    : default( IList<SqlParameter> );
                            }
                        }

                        return default( IList<DbParameter> );
                    }
                }
                catch( Exception ex )
                {
                    DataRowExtensions.Fail( ex );
                    return default( IList<DbParameter> );
                }
            }

            return default( IList<DbParameter> );
        }

        /// <summary>
        /// Converts to dictionary.
        /// </summary>
        /// <param name="dataRow">The data row.</param>
        /// <returns></returns>
        public static IDictionary<string, object> ToDictionary( this DataRow dataRow )
        {
            try
            {
                if( dataRow?.ItemArray.Length > 0 )
                {
                    var _dictionary = new Dictionary<string, object>( );
                    for( var _i = 0; _i < dataRow.ItemArray.Length; _i++ )
                    {
                        var _name = dataRow.Table.Columns[ _i ].ColumnName;
                        var _value = dataRow?.ItemArray[ _i ];
                        if( !string.IsNullOrEmpty( _name )
                            && _value != null )
                        {
                            _dictionary?.Add( _name, _value );
                        }
                    }

                    return _dictionary?.Keys?.Count > 0
                        ? _dictionary
                        : default( IDictionary<string, object> );
                }

                return default( IDictionary<string, object> );
            }
            catch( Exception ex )
            {
                DataRowExtensions.Fail( ex );
                return default( IDictionary<string, object> );
            }
        }

        /// <summary>
        /// Converts to namevaluecollection.
        /// </summary>
        /// <param name="dataRow">The data row.</param>
        /// <returns>
        /// SortedList(string, object)
        /// </returns>
        public static SortedList<int, KeyValuePair<string, object>> ToSortedList(
            this DataRow dataRow )
        {
            try
            {
                if( dataRow?.ItemArray.Length > 0 )
                {
                    var _sortedList = new SortedList<int, KeyValuePair<string, object>>( );
                    var _items = dataRow?.ItemArray;
                    for( var _i = 0; _i < dataRow?.ItemArray.Length; _i++ )
                    {
                        var _key = dataRow?.Table.Columns[ _i ].ColumnName;
                        if( _items[ _i ] != null
                            && !string.IsNullOrEmpty( _key ) )
                        {
                            var _kvp = new KeyValuePair<string, object>( _key, _items[ _i ] );
                            _sortedList?.Add( _i, _kvp );
                        }
                    }

                    return _sortedList?.Count > 0
                        ? _sortedList
                        : default( SortedList<int, KeyValuePair<string, object>> );
                }

                return default( SortedList<int, KeyValuePair<string, object>> );
            }
            catch( Exception ex )
            {
                DataRowExtensions.Fail( ex );
                return default( SortedList<int, KeyValuePair<string, object>> );
            }
        }

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <param name="dataRow">The data row.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns>
        /// IEnumerable
        /// </returns>
        public static IEnumerable<byte> GetBytes( this DataRow dataRow, string columnName )
        {
            try
            {
                ThrowIf.Null( columnName, nameof( columnName ) );
                return dataRow[ columnName ] as byte[ ];
            }
            catch( Exception ex )
            {
                DataRowExtensions.Fail( ex );
                return default( IEnumerable<byte> );
            }
        }

        /// <summary>
        /// Iterates the items.
        /// </summary>
        /// <param name="dataRow">The data row.</param>
        /// <returns>
        /// IEnumerator(object)
        /// </returns>
        public static IEnumerator<object> IterItems( this DataRow dataRow )
        {
            foreach( var _item in dataRow.ItemArray )
            {
                if( _item != null )
                {
                    yield return _item;
                }
            }
        }

        /// <summary>
        /// Fails the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        private static void Fail( Exception ex )
        {
            var _error = new ErrorWindow( ex );
            _error?.SetText( );
            _error?.ShowDialog( );
        }
    }
}