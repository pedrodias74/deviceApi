use [DeviceDatabase]
go

-- drops table if exists
if  exists (select * from sys.objects where object_id = object_id(N'[dbo].[Device]') and type in (N'U'))
	drop table [dbo].[Device]
go


set ansi_nulls on
go

set quoted_identifier on
go

-- creates the table
create table [dbo].[Device](
	[Id] [int] identity(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Brand] [nvarchar](50) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](250) NULL,
	[UpdatedOn] [datetime] NULL,
	[UpdatedBy] [nvarchar](250) NULL,
	constraint [PK_Device] primary key clustered ([Id] asc) 
	with (	
		pad_index = off, 
		statistics_norecompute = off, 
		ignore_dup_key = off, 
		allow_row_locks = on, 
		allow_page_locks = on, 
		optimize_for_sequential_key = off
	) on [primary]
) on [primary]
go

-- creates brand column index
create nonclustered index IX_Device_Brand on [Device]([Brand]) 
go

