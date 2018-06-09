
USE [master]
GO

WHILE EXISTS(select NULL from sys.databases where name='QLNT')
BEGIN
    DECLARE @SQL varchar(max)
    SELECT @SQL = COALESCE(@SQL,'') + 'Kill ' + Convert(varchar, SPId) + ';'
    FROM MASTER..SysProcesses
    WHERE DBId = DB_ID(N'QLNT') AND SPId <> @@SPId
    EXEC(@SQL)
    DROP DATABASE [QLNT]
END
GO

/* Collation = SQL_Latin1_General_CP1_CI_AS */
CREATE DATABASE [QLNT]
GO

USE [QLNT]
GO

CREATE TABLE [dbo].[tblTreEm](
	[MaTreEm] [nvarchar](20) NOT NULL,
	[HoTenTreEm] [nvarchar](50) NOT NULL,
	[NgaySinh] [smalldatetime] NOT NULL,
	[HoTenPhuHuynh] [nvarchar](50) NOT NULL,
	[TenONha] [nvarchar](50) NOT NULL,
	[DiaChi] [nvarchar](50) NOT NULL,
	[DienThoai] [nvarchar](20) NOT NULL,
	[Tuoi] [int] NULL,
	[NgayNhapHoc] [smalldatetime] NULL,
	[GhiChu] [nvarchar](50) NULL,
	[MaLop] [nvarchar](20) NULL,/*co the de trong do nhap hoc sinh ko can nhap lop*/
 CONSTRAINT [PK_tblTreEm] PRIMARY KEY CLUSTERED 
(
	[MaTreEm] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [QLNT]
GO


CREATE TABLE [dbo].[tblLop](
	[MaLop] [nvarchar](20) NOT NULL,
	[TenLop] [nvarchar](20) NOT NULL,
	[MaKhoi] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_tblLop] PRIMARY KEY CLUSTERED 
(
	[MaLop] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [QLNT]
GO


CREATE TABLE [dbo].[tblKhoi](
	[MaKhoi] [nvarchar](20) NOT NULL,
	[TenKhoi] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_tblKhoi] PRIMARY KEY CLUSTERED 
(
	[MaKhoi] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


USE [QLNT]
GO

CREATE TABLE [dbo].[tblTinhTrang](
	[MaTinhTrang] [nvarchar](20) NOT NULL,
	[TenTinhTrang] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_tblTinhTrang] PRIMARY KEY CLUSTERED 
(
	[MaTinhTrang] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [QLNT]
GO


CREATE TABLE [dbo].[tblGhiNhanTinhTrang](
	[MaPhieuGhiNhan] [nvarchar](20) NOT NULL,
	[MaTreEm] [nvarchar](20) NOT NULL,
	[MaTinhTrang] [nvarchar](20) NOT NULL,
	[NgayGhiNhan] [smalldatetime] NOT NULL,
	[MaKhoi] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_tblGhiNhanTinhTrang] PRIMARY KEY CLUSTERED 
(
	[MaPhieuGhiNhan] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [QLNT]
GO


CREATE TABLE [dbo].[tblBaoCao](
	[MaBaoCao] [nvarchar](20) NOT NULL,
	[NgayBaoCao] [smalldatetime] NOT NULL,
	[MaKhoi] [nvarchar](20) NULL,
 CONSTRAINT [PK_tblBaoCao] PRIMARY KEY CLUSTERED 
(
	[MaBaoCao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [QLNT]
GO


USE [QLNT]
GO


CREATE TABLE [dbo].[tblChiTietBaoCao](
	[MaChiTietBaoCao] [nvarchar](20) NOT NULL,
	[MaPhieuGhiNhan] [nvarchar](20) NOT NULL,
	[MaBaoCao] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_tblChiTietBaoCao] PRIMARY KEY CLUSTERED 
(
	[MaChiTietBaoCao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


USE [QLNT]
GO

CREATE TABLE [dbo].[tblThamSo](
	[TuoiToiTieu] [int] NOT NULL,
	[TuoiToiDa] [int] NOT NULL,
	[SoHocSinhToiDa] [int] NOT NULL
) ON [PRIMARY]
GO


USE [QLNT]
GO

INSERT INTO[dbo].[tblThamSo]([TuoiToiTieu],[TuoiToiDa],[SoHocSinhToiDa]) VALUES(3,5,20)
GO


USE [QLNT]
GO
ALTER TABLE dbo.tblLop ADD FOREIGN KEY(MaKhoi) REFERENCES dbo.tblKhoi(MaKhoi)

GO

USE [QLNT]
GO
ALTER TABLE dbo.tblTreEm ADD FOREIGN KEY(MaLop) REFERENCES dbo.tblLop(MaLop)

GO

USE [QLNT]
GO
ALTER TABLE dbo.tblGhiNhanTinhTrang ADD FOREIGN KEY(MaTreEm) REFERENCES dbo.tblTreEm(MaTreEm)

GO

USE [QLNT]
GO
ALTER TABLE dbo.tblGhiNhanTinhTrang ADD FOREIGN KEY(MaTinhTrang) REFERENCES dbo.tblTinhTrang(MaTinhTrang)

GO

USE [QLNT]
GO
ALTER TABLE dbo.tblGhiNhanTinhTrang ADD FOREIGN KEY(MaKhoi) REFERENCES dbo.tblKhoi(MaKhoi)

GO

USE [QLNT]
GO
ALTER TABLE dbo.tblChiTietBaoCao ADD FOREIGN KEY(MaPhieuGhiNhan) REFERENCES dbo.tblGhiNhanTinhTrang(MaPhieuGhiNhan)

GO

USE [QLNT]
GO
ALTER TABLE dbo.tblChiTietBaoCao ADD FOREIGN KEY(MaBaoCao) REFERENCES dbo.tblBaoCao(MaBaoCao)

GO
