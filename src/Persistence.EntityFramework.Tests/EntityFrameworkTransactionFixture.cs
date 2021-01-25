#region Copyright & License

// Copyright © 2020 - 2021 Emmanuel Benitez
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using Xunit;

namespace BigSolution.Persistence
{
    public class EntityFrameworkTransactionFixture
    {
        [Fact]
        public void CommitAsyncSucceeds()
        {
            var mockedTransaction = new Mock<IDbContextTransaction>();
            var transaction = new EntityFrameworkTransaction(mockedTransaction.Object);
            var cancellationToken = new CancellationToken();
            transaction.CommitAsync(cancellationToken).Wait(cancellationToken);
            mockedTransaction.Verify(contextTransaction => contextTransaction.CommitAsync(It.IsIn(cancellationToken)), Times.Once);
        }

        [Fact]
        public void CommitSucceeds()
        {
            var mockedTransaction = new Mock<IDbContextTransaction>();
            var transaction = new EntityFrameworkTransaction(mockedTransaction.Object);

            transaction.Commit();
            mockedTransaction.Verify(contextTransaction => contextTransaction.Commit(), Times.Once);
        }

        [Fact]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void CreateFailed()
        {
            Action action = () => new EntityFrameworkTransaction(null);
            action.Should().ThrowExactly<ArgumentNullException>().Where(exception => exception.ParamName == "transaction");
        }

        [Fact]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void CreateSucceeds()
        {
            Action action = () => new EntityFrameworkTransaction(new Mock<IDbContextTransaction>().Object);
            action.Should().NotThrow();
        }

        [Fact]
        public void DisposeSucceeds()
        {
            var mockedTransaction = new Mock<IDbContextTransaction>();
            using (new EntityFrameworkTransaction(mockedTransaction.Object)) { }
            mockedTransaction.Verify(transaction => transaction.Dispose(), Times.Once);
        }

        [Fact]
        public void RollbackAsyncSucceeds()
        {
            var mockedTransaction = new Mock<IDbContextTransaction>();
            var transaction = new EntityFrameworkTransaction(mockedTransaction.Object);
            var cancellationToken = new CancellationToken();
            transaction.RollbackAsync(cancellationToken).Wait(cancellationToken);
            mockedTransaction.Verify(contextTransaction => contextTransaction.RollbackAsync(It.IsIn(cancellationToken)), Times.Once);
        }

        [Fact]
        public void RollbackSucceeds()
        {
            var mockedTransaction = new Mock<IDbContextTransaction>();
            var transaction = new EntityFrameworkTransaction(mockedTransaction.Object);

            transaction.Rollback();
            mockedTransaction.Verify(contextTransaction => contextTransaction.Rollback(), Times.Once);
        }
    }
}
