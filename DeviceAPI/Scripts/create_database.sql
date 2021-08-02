use [master]
go

-- drops database if exists
if  exists (select * from sys.databases where [name] = N'DeviceDatabase')
begin
	alter database [DeviceDatabase] set single_user with rollback immediate
	drop database [DeviceDatabase]
end
go

declare @DefaultDataPath nvarchar(max)
set @DefaultDataPath = (select convert(nvarchar(max), serverproperty('INSTANCEDEFAULTDATAPATH')))


-- creates database
create database [DeviceDatabase]
	CONTAINMENT = none
	with CATALOG_COLLATION = DATABASE_DEFAULT
go

alter database [DeviceDatabase] modify file
( name = N'DeviceDatabase', MAXSIZE = unlimited, FILEGROWTH = 65536KB )
go

alter database [DeviceDatabase] modify file
( name = N'DeviceDatabase_log', MAXSIZE = 2048GB, FILEGROWTH = 65536KB )
go

if (1 = fulltextserviceproperty('IsFullTextInstalled'))
begin
	exec [DeviceDatabase].[dbo].[sp_fulltext_database] @action = 'enable'
end
go

alter database [DeviceDatabase] set ansi_null_default off 
go

alter database [DeviceDatabase] set ansi_nulls off 
go

alter database [DeviceDatabase] set ansi_padding off 
go

alter database [DeviceDatabase] set ansi_warnings off 
go

alter database [DeviceDatabase] set arithabort off 
go

alter database [DeviceDatabase] set auto_close off 
go

alter database [DeviceDatabase] set auto_shrink off 
go

alter database [DeviceDatabase] set auto_update_statistics on 
go

alter database [DeviceDatabase] set cursor_close_on_commit off 
go

alter database [DeviceDatabase] set cursor_default  global 
go

alter database [DeviceDatabase] set concat_null_yields_null off 
go

alter database [DeviceDatabase] set numeric_roundabort off 
go

alter database [DeviceDatabase] set quoted_identifier off 
go

alter database [DeviceDatabase] set recursive_triggers off 
go

alter database [DeviceDatabase] set  disable_broker 
go

alter database [DeviceDatabase] set auto_update_statistics_async off 
go

alter database [DeviceDatabase] set date_correlation_optimization off 
go

alter database [DeviceDatabase] set trustworthy off 
go

alter database [DeviceDatabase] set allow_snapshot_isolation off 
go

alter database [DeviceDatabase] set parameterization simple 
go

alter database [DeviceDatabase] set read_committed_snapshot off 
go

alter database [DeviceDatabase] set honor_broker_priority off 
go

alter database [DeviceDatabase] set recovery full 
go

alter database [DeviceDatabase] set  multi_user 
go

alter database [DeviceDatabase] set page_verify checksum  
go

alter database [devicedatabase] set db_chaining off 
go

alter database [DeviceDatabase] set filestream( non_transacted_access = off ) 
go

alter database [DeviceDatabase] set target_recovery_time = 60 seconds 
go

alter database [DeviceDatabase] set delayed_durability = disabled 
go

alter database [DeviceDatabase] set accelerated_database_recovery = off  
go

alter database [DeviceDatabase] set query_store = off
go

alter database [DeviceDatabase] set  read_write 
go


