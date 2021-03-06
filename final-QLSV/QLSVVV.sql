Create Database QLSinhVien
go

use QLSinhVien
go

--Bảng Môn học--
Create Table MONHOC
 (
   
   MaMH nvarchar(5) primary key,
   TenMH nvarchar(30) not null,
   SoTC int not null check ( (SoTC>0)and (SoTC<10) )
 )
 
 --Bảng Hệ đào tạo--
 Create Table HEDAOTAO
 (
   MaHDT char(5) primary key,
   TenHDT nvarchar(40) not null
 )

--Bảng Khóa học--
Create Table KHOAHOC
 (
   MaKhoaHoc char(5) primary key,
   TenKhoaHoc nvarchar(20) not null
 )
 
 --Bảng Khoa--
 Create Table KHOA
 (
   MaKhoa char(5) primary key,
   TenKhoa nvarchar(30) not null,
   DiaChi nvarchar(100) not null,
   DienThoai varchar(20) not null
 )
 
 --Bảng Lớp học--
 Create Table LOP
 (
   MaLop char(5) primary key,
   TenLop nvarchar(30) not null,
   MaKhoa char(5) foreign key references KHOA (MaKhoa),
   MaHDT char(5) foreign key references HEDAOTAO (MaHDT),
   MaKhoaHoc char(5) foreign key references KHOAHOC (MaKhoaHoc),
 )
 
 --Bảng Sinh viên--
 Create Table SINHVIEN
 (
   MaSV nvarchar(15) primary key,
   TenSV nvarchar(20) ,
   GioiTinh nvarchar(10) ,
   NgaySinh datetime ,
   QueQuan nvarchar(50) ,
   ChuyenNganh nvarchar(50),
   SDT bigint,
   CMND bigint,
   NoiThuongTru nvarchar(50),
   MaKhoa char(5) foreign key references KHOA (MaKhoa),
   MaKhoaHoc char(5) foreign key references KHOAHOC (MaKhoaHoc),
   MaLop char(5) foreign key references LOP(MaLop)
 )
 
 --Bảng Điểm sinh viên--
 Create Table DIEM
 (
   Stt bigint IDENTITY(1, 1) primary key,
   MaSV nvarchar(15) foreign key references SINHVIEN(MaSV),
   MaMH nvarchar(5) foreign key references MONHOC(MaMH),
   HocKy int check(HocKy>0) not null,
   DiemLan1 float ,
   DiemLan2 float,
   DiemTB float,

)

--Bảng Tài khoản--
Create Table TAIKHOAN
(
	TenDN nvarchar(15) foreign key references SINHVIEN(MaSV),
	MatKhau nvarchar(30),
	primary key(TenDN)
	
)

Create Table ADMIN
(
	
	Username nvarchar(15) primary key,
	Password nvarchar(30) not null,
	Name nvarchar(50),
	Role bigint
)

--Dữ liệu Môn học--
insert into MONHOC values('MLN', N'Mác-Lênin', 5)
insert into MONHOC values('TA', N'Tiếng Anh', 3)
insert into MONHOC values('LTHDT', N'Lập Trình Hướng Đối Tượng', 5)
insert into MONHOC values('TCC', N'Toán Cao Cấp', 3)

select * from MONHOC
 
 ---Dữ liệu Hệ đào tạo --
insert into HEDAOTAO values('DH01',N'Ðại Học')
insert into HEDAOTAO values('CD01',N'Cao Ðẳng')
insert into HEDAOTAO values('TC01',N'Trung Cấp')

Select * from HEDAOTAO

-- Dữ liệu Khóa học ---
insert into KHOAHOC values('K15',N'ÐH khóa 2015-2019')
insert into KHOAHOC values('K16',N'ÐH khóa 2016-2020')
insert into KHOAHOC values('K17',N'ÐH khóa 2018-2021')
insert into KHOAHOC values('E15',N'CĐ khóa 2015-2017')
insert into KHOAHOC values('E16',N'CĐ khóa 2016-2018')
insert into KHOAHOC values('T15',N'TC khóa 2015-2018')

Select * from KHOAHOC


-- Dữ liệu Khoa --
insert into KHOA values('CNTT',N'Công nghệ thông tin',N'Tầng 2 khu B','043768888')
insert into KHOA values('CK',N'Cơ Khí',N'Tầng 2 khu B','043769999')
insert into KHOA values('DT',N'Ðiện tử',N'Tằng 3 khu A','043767777')
insert into KHOA values('KT',N'Kinh Tế',N'Tầng 2 khu U','043762222')

Select * from KHOA

--- Dữ liệu Lớp --
insert into LOP values('DTH',N'15DTH13','CNTT','DH01','K15')
insert into LOP values('DTO',N'15DTH14','CNTT','DH01','K15')
insert into LOP values('QTK',N'14QTK02','KT','DH01','K17')
insert into LOP values('CKK',N'16CKK01','CK','CD01','E16')
insert into LOP values('DTT',N'17DTT07','DT','CD01','E16')

select * from LOP

-- Dữ liệu Sinh viên --
insert into SINHVIEN values('1511061082',N'Phan Hồ Trọng Hiếu','Nam','06/26/1997',N'Đồng Tháp',N'Công Nghệ Phần Mềm', 0919666970, 341898868, N'Điện Biên Phủ', 'CNTT', 'K15', 'DTH')
insert into SINHVIEN values('1511001084',N'Nguyễn Phú Quý',N'Nữ','07/21/1995',N'Nha Trang',N'Xây dựng', 0913224442, 017298864, N'Lê Thánh Tông', 'CK', 'K15', 'CKK')
insert into SINHVIEN values('1522012017',N'Dương Quốc Hào',N'Nữ','02/24/1996',N'Tây Ninh',N'Kế Toán', 0887451121, 247568142, N'Thị Nghè', 'KT', 'K16', 'QTK')


select * from SINHVIEN


--Dữ liệu điểm--
insert into DIEM values('1511061082', 'LTHDT', 2, 9, 10, 9.5)
insert into DIEM values('1511001084', 'MLN', 1, 7, 9, 8)
insert into DIEM values('1522012017', 'TA', 2, 8, 10, 9)
insert into DIEM values('1522012017', 'TCC', 2, 8, 10, 9)
insert into DIEM values('1511061082', 'TCC', 2, 7, 10, 8.5)

select * from DIEM
select s.TenSV, HocKy, DiemLan1, DiemLan2, DiemTB from DIEM a, SINHVIEN s where a.MaSV = s.MaSV
delete from DIEM where Stt = 1
--Dữ liệu Tài khoản--
insert into TAIKHOAN values('1511061082', 'hieu123')
insert into TAIKHOAN values('1522012017', 'hao123')

select * from TAIKHOAN

--Dữ liệu Admin--
insert into ADMIN values('admin', 'admin123', 'Riot', 1)
insert into ADMIN values('gvphth', 'phth123', N'Trọng Hiếu', 2)
insert into ADMIN values('gvdqh', 'dqh123', 'Quốc Hào', 2)


select * from ADMIN




				-----------POST TIN TỨC-------------
				
--Bảng danh mục bài viết--
Create Table DANHMUC
(
	Id bigint IDENTITY(1,1) primary key,
	TenDM nvarchar(50) not null	
)

--Bảng bài viết chi tiết--
Create Table BAIVIET
(
	IdBV bigint IDENTITY(1, 1) primary key,
	Id bigint foreign key references DANHMUC(Id),
	TieuDe nvarchar(500) not null,
	HinhAnh varchar(50),
	TomTat nvarchar(500) not null,
	NgayViet datetime not null,
	ChiTiet nvarchar(3000) not null,
	Nguon nvarchar(50)
)

--Dữ liệu danh mục--
insert into DANHMUC values(N'Đội ngũ chuyên gia')
insert into DANHMUC values(N'Chương trình học')
insert into DANHMUC values(N'Tin tức')
insert into DANHMUC values(N'Thành tựu')
insert into DANHMUC values(N'Đội ngũ liên kết')
insert into DANHMUC values(N'Hỗ trợ việc học')
insert into DANHMUC values(N'Giới thiệu')

select * from DANHMUC


--Dữ liệu bài viết--
insert into BAIVIET values(2, N'Đại học Cergy-Pontoise trao bằng Cử nhân Quản trị nhà hàng và dịch vụ ăn uống quốc tế tại HUTECH', 'hutech1.jpg', N'Sáng 7/4 vừa qua, trường Đại học Cergy-Pontoise (Pháp) đã phối hợp với Viện Đào tạo quốc tế - trường Đại học Công nghệ TP.HCM (Đại học HUTECH) trang trọng tổ chức Lễ tốt nghiệp cho các tân Cử nhân thuộc chương trình Quản trị nhà hàng và dịch vụ ăn uống quốc tế do ĐH Cergy-Pontoise và HUTECH hợp tác đào tạo.', '05/20/2018', N'Sáng 7/4 vừa qua, trường Đại học Cergy-Pontoise (Pháp) đã phối hợp với Viện Đào tạo quốc tế - trường Đại học Công nghệ TP.HCM (Đại học HUTECH) trang trọng tổ chức Lễ tốt nghiệp cho các tân Cử nhân thuộc chương trình Quản trị nhà hàng và dịch vụ ăn uống quốc tế do ĐH Cergy-Pontoise và HUTECH hợp tác đào tạo.
Tham dự buỗi lễ có GS.TS Eric Olemedo - đại diện trường ĐH Cergy-Pontoise, ông Felipe Diaz Marin - Quản lý Chương trình Giáo dục Đại học Cergy-Pontoise và bà Jeanne Cottenceau - đại diện từ Lãnh sự quán Pháp tại TP.HCM. Về phía trường Đại học HUTECH, buổi lễ có TS. Kiều Tuân - Chủ tịch HĐQT Nhà trường, PGS.TS. Bùi Xuân Lâm - Phó Hiệu trưởng, TS. Phạm Thị Hải Quỳnh và ThS. Nguyễn Lan Hương - Phó Viện trưởng Viện Đào tạo quốc tế HUTECH. Buổi lễ còn có sự tham dự của quý phụ huynh, bạn bè của các tân Cử nhân đến chia vui trong khoảnh khắc trang trọng này.', N'Đại học Hutech')

select * from BAIVIET

select * from BAIVIET where IdBV = 1 and IdBV = 2