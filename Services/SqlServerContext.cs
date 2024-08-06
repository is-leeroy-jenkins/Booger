// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 08-05-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-05-2024
// ******************************************************************************************
// <copyright file="SqlServerContext.cs" company="Terry D. Eppler">
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
//   SqlServerContext.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    using System;
    using Microsoft.EntityFrameworkCore;

    // Reference ('Code First'): 
    // PM> Add-Migration InitialCreate (set to 'Any CPU', not x86)
    // PM> Update-Database 
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:Microsoft.EntityFrameworkCore.DbContext" />
    public class SqlServerContext : DbContext
    {
        /// <summary>
        /// The connection string
        /// </summary>
        private readonly string _connectionString;

        // This ctor is needed for PM> Update-Database
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Booger.SqlServerContext" /> class.
        /// </summary>
        /// <remarks>
        /// See <see href="https://aka.ms/efcore-docs-dbcontext">
        /// DbContext lifetime, configuration, and initialization</see>
        /// for more information and examples.
        /// </remarks>
        public SqlServerContext( )
        {
            _connectionString = SqlHistoryRepo._sqlConnectionString;
        }

        // DbSet maps a table in DB
        /// <summary>
        /// Gets or sets the history chat.
        /// </summary>
        /// <value>
        /// The history chat.
        /// </value>
        public DbSet<HistoryChat> HistoryChat { get; set; }

        /// <summary>
        /// Gets or sets the history message.
        /// </summary>
        /// <value>
        /// The history message.
        /// </value>
        public DbSet<HistoryMessage> HistoryMessage { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// Override this method to configure the database (and other options) to be used for this context.
        /// This method is called for each instance of the context that is created.
        /// The base implementation does nothing.
        /// </summary>
        /// <param name="optionsBuilder">A builder used to create or
        /// modify options for this context.
        /// Databases (and other extensions)
        /// typically define extension methods on this object
        /// that allow you to configure the context.</param>
        /// <remarks>
        /// <para>
        /// In situations where an instance of
        /// <see cref="T:Microsoft.EntityFrameworkCore.DbContextOptions" />
        /// may or may not have been passed
        /// to the constructor, you can use
        /// <see cref="P:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.IsConfigured" />
        /// to determine if
        /// the options have already been set, and skip some or all of the logic in
        /// </para>
        /// <para>
        /// See <see href="https://aka.ms/efcore-docs-dbcontext">DbContext lifetime, configuration, and initialization</see>
        /// for more information and examples.
        /// </para>
        /// </remarks>
        protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
        {
            base.OnConfiguring( optionsBuilder );
            optionsBuilder.UseSqlServer( _connectionString );
        }

        /// <summary>
        /// Fails the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        private protected void Fail( Exception ex )
        {
            var _error = new ErrorWindow( ex );
            _error?.SetText( );
            _error?.ShowDialog( );
        }
    }
}