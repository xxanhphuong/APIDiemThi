USE [master]
GO
/****** Object:  Database [APIDiemThi]    Script Date: 27/10/2021 11:51:59 am ******/
CREATE DATABASE [APIDiemThi]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'APIDiemThi', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\APIDiemThi.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'APIDiemThi_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\APIDiemThi_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [APIDiemThi] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [APIDiemThi].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [APIDiemThi] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [APIDiemThi] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [APIDiemThi] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [APIDiemThi] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [APIDiemThi] SET ARITHABORT OFF 
GO
ALTER DATABASE [APIDiemThi] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [APIDiemThi] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [APIDiemThi] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [APIDiemThi] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [APIDiemThi] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [APIDiemThi] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [APIDiemThi] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [APIDiemThi] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [APIDiemThi] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [APIDiemThi] SET  ENABLE_BROKER 
GO
ALTER DATABASE [APIDiemThi] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [APIDiemThi] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [APIDiemThi] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [APIDiemThi] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [APIDiemThi] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [APIDiemThi] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [APIDiemThi] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [APIDiemThi] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [APIDiemThi] SET  MULTI_USER 
GO
ALTER DATABASE [APIDiemThi] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [APIDiemThi] SET DB_CHAINING OFF 
GO
ALTER DATABASE [APIDiemThi] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [APIDiemThi] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [APIDiemThi] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [APIDiemThi] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [APIDiemThi] SET QUERY_STORE = OFF
GO
USE [APIDiemThi]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 27/10/2021 11:51:59 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Classes]    Script Date: 27/10/2021 11:51:59 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Classes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Quantity] [int] NOT NULL,
	[MajorId] [int] NOT NULL,
 CONSTRAINT [PK_Classes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Major]    Script Date: 27/10/2021 11:51:59 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Major](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[StartDate] [datetime2](7) NOT NULL,
	[FinishDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Major] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Score]    Script Date: 27/10/2021 11:51:59 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Score](
	[StudentId] [int] NOT NULL,
	[SubjectId] [int] NOT NULL,
	[MidScore] [float] NULL,
	[FinalScore] [float] NULL,
 CONSTRAINT [PK_Score] PRIMARY KEY CLUSTERED 
(
	[StudentId] ASC,
	[SubjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Student]    Script Date: 27/10/2021 11:51:59 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student](
	[StudentId] [int] NOT NULL,
	[TypeStudent] [int] NOT NULL,
	[ClassesId] [int] NOT NULL,
 CONSTRAINT [PK_Student] PRIMARY KEY CLUSTERED 
(
	[StudentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Subject]    Script Date: 27/10/2021 11:51:59 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subject](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Credit] [int] NOT NULL,
	[MidScoreRatio] [float] NOT NULL,
	[FinalScoreRatio] [float] NOT NULL,
	[TeacherId] [int] NOT NULL,
 CONSTRAINT [PK_Subject] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Teacher]    Script Date: 27/10/2021 11:51:59 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Teacher](
	[TeacherId] [int] NOT NULL,
	[Salary] [float] NOT NULL,
 CONSTRAINT [PK_Teacher] PRIMARY KEY CLUSTERED 
(
	[TeacherId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 27/10/2021 11:51:59 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](max) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[Role] [nvarchar](max) NOT NULL,
	[FullName] [nvarchar](max) NOT NULL,
	[DateOfBirth] [datetime2](7) NOT NULL,
	[Gender] [nvarchar](max) NOT NULL,
	[Address] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20211019100845_addDBStudent', N'5.0.11')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20211019151320_updatedb1', N'5.0.11')
GO
SET IDENTITY_INSERT [dbo].[Classes] ON 

INSERT [dbo].[Classes] ([Id], [Name], [Quantity], [MajorId]) VALUES (1, N'DH18IT82', 89, 1)
SET IDENTITY_INSERT [dbo].[Classes] OFF
GO
SET IDENTITY_INSERT [dbo].[Major] ON 

INSERT [dbo].[Major] ([Id], [Name], [StartDate], [FinishDate]) VALUES (1, N'Công Nghệ Thông Tin K18', CAST(N'2018-09-19T16:21:08.5550000' AS DateTime2), CAST(N'2022-10-19T16:21:08.5550000' AS DateTime2))
INSERT [dbo].[Major] ([Id], [Name], [StartDate], [FinishDate]) VALUES (2, N'Khoa Học Máy Tính K18', CAST(N'2018-09-19T16:21:08.5550000' AS DateTime2), CAST(N'2022-10-19T16:21:08.5550000' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Major] OFF
GO
INSERT [dbo].[Score] ([StudentId], [SubjectId], [MidScore], [FinalScore]) VALUES (1, 1, 10, 10)
INSERT [dbo].[Score] ([StudentId], [SubjectId], [MidScore], [FinalScore]) VALUES (1, 2, 0, 0)
GO
INSERT [dbo].[Student] ([StudentId], [TypeStudent], [ClassesId]) VALUES (1, 1, 1)
GO
SET IDENTITY_INSERT [dbo].[Subject] ON 

INSERT [dbo].[Subject] ([Id], [Name], [Credit], [MidScoreRatio], [FinalScoreRatio], [TeacherId]) VALUES (1, N'Lập Trình C++', 4, 30, 70, 3)
INSERT [dbo].[Subject] ([Id], [Name], [Credit], [MidScoreRatio], [FinalScoreRatio], [TeacherId]) VALUES (2, N'Lập Trình Giao Diện', 4, 30, 70, 3)
SET IDENTITY_INSERT [dbo].[Subject] OFF
GO
INSERT [dbo].[Teacher] ([TeacherId], [Salary]) VALUES (3, 12000000)
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([Id], [Username], [Password], [Role], [FullName], [DateOfBirth], [Gender], [Address]) VALUES (1, N'mymy123', N'dq+YNuNesVRYSlGsF8xMfQ==', N'student', N'Trà My', CAST(N'2021-10-20T03:03:19.5880000' AS DateTime2), N'Nữ', N'Bạc Liêu')
INSERT [dbo].[Users] ([Id], [Username], [Password], [Role], [FullName], [DateOfBirth], [Gender], [Address]) VALUES (2, N'admin', N'gypwSo7j4jZVgTcx/ABo7w==', N'admin', N'Huỳnh Linh Khôi', CAST(N'2021-10-20T03:08:39.0350000' AS DateTime2), N'Nam', N'Bạc Liêu')
INSERT [dbo].[Users] ([Id], [Username], [Password], [Role], [FullName], [DateOfBirth], [Gender], [Address]) VALUES (3, N'thao12', N'gypwSo7j4jZVgTcx/ABo7w==', N'teacher', N'Nguyễn Linh Thảo', CAST(N'2021-10-20T03:08:39.0350000' AS DateTime2), N'Nữ', N'Bạc Liêu')
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
/****** Object:  Index [IX_Classes_MajorId]    Script Date: 27/10/2021 11:51:59 am ******/
CREATE NONCLUSTERED INDEX [IX_Classes_MajorId] ON [dbo].[Classes]
(
	[MajorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Score_SubjectId]    Script Date: 27/10/2021 11:51:59 am ******/
CREATE NONCLUSTERED INDEX [IX_Score_SubjectId] ON [dbo].[Score]
(
	[SubjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Student_ClassesId]    Script Date: 27/10/2021 11:51:59 am ******/
CREATE NONCLUSTERED INDEX [IX_Student_ClassesId] ON [dbo].[Student]
(
	[ClassesId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Subject_TeacherId]    Script Date: 27/10/2021 11:51:59 am ******/
CREATE NONCLUSTERED INDEX [IX_Subject_TeacherId] ON [dbo].[Subject]
(
	[TeacherId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Classes] ADD  DEFAULT (N'') FOR [Name]
GO
ALTER TABLE [dbo].[Major] ADD  DEFAULT (N'') FOR [Name]
GO
ALTER TABLE [dbo].[Subject] ADD  DEFAULT (N'') FOR [Name]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (N'') FOR [Username]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (N'') FOR [Password]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (N'') FOR [Role]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (N'') FOR [FullName]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (N'') FOR [Gender]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (N'') FOR [Address]
GO
ALTER TABLE [dbo].[Classes]  WITH CHECK ADD  CONSTRAINT [FK_Classes_Major_MajorId] FOREIGN KEY([MajorId])
REFERENCES [dbo].[Major] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Classes] CHECK CONSTRAINT [FK_Classes_Major_MajorId]
GO
ALTER TABLE [dbo].[Score]  WITH CHECK ADD  CONSTRAINT [FK_Score_Student_StudentId] FOREIGN KEY([StudentId])
REFERENCES [dbo].[Student] ([StudentId])
GO
ALTER TABLE [dbo].[Score] CHECK CONSTRAINT [FK_Score_Student_StudentId]
GO
ALTER TABLE [dbo].[Score]  WITH CHECK ADD  CONSTRAINT [FK_Score_Subject_SubjectId] FOREIGN KEY([SubjectId])
REFERENCES [dbo].[Subject] ([Id])
GO
ALTER TABLE [dbo].[Score] CHECK CONSTRAINT [FK_Score_Subject_SubjectId]
GO
ALTER TABLE [dbo].[Student]  WITH CHECK ADD  CONSTRAINT [FK_Student_Classes_ClassesId] FOREIGN KEY([ClassesId])
REFERENCES [dbo].[Classes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Student] CHECK CONSTRAINT [FK_Student_Classes_ClassesId]
GO
ALTER TABLE [dbo].[Student]  WITH CHECK ADD  CONSTRAINT [FK_Student_Users_StudentId] FOREIGN KEY([StudentId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Student] CHECK CONSTRAINT [FK_Student_Users_StudentId]
GO
ALTER TABLE [dbo].[Subject]  WITH CHECK ADD  CONSTRAINT [FK_Subject_Teacher_TeacherId] FOREIGN KEY([TeacherId])
REFERENCES [dbo].[Teacher] ([TeacherId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Subject] CHECK CONSTRAINT [FK_Subject_Teacher_TeacherId]
GO
ALTER TABLE [dbo].[Teacher]  WITH CHECK ADD  CONSTRAINT [FK_Teacher_Users_TeacherId] FOREIGN KEY([TeacherId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Teacher] CHECK CONSTRAINT [FK_Teacher_Users_TeacherId]
GO
USE [master]
GO
ALTER DATABASE [APIDiemThi] SET  READ_WRITE 
GO
