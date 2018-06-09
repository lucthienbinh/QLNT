﻿Imports System.Configuration
Imports System.Data.SqlClient
Imports QLNT_DTO
Imports Utility
Public Class TreEmDAL
    Private connectionString As String

    Public Sub New()
        ' Read ConnectionString value from App.config file
        connectionString = ConfigurationManager.AppSettings("ConnectionString")
    End Sub
    Public Sub New(ConnectionString As String)
        Me.connectionString = ConnectionString
    End Sub
    Public Function buildMSTE(ByRef nextMste As String) As Result 'ex: 180001

        nextMste = String.Empty
        Dim y = DateTime.Now.Year
        Dim x = y.ToString().Substring(2)
        nextMste = x + "0000"

        Dim query As String = String.Empty
        query &= "SELECT TOP 1 [MaTreEm] "
        query &= "FROM [tblTreEm] "
        query &= "ORDER BY [MaTreEm] DESC "

        Using conn As New SqlConnection(connectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                End With
                Try
                    conn.Open()
                    Dim reader As SqlDataReader
                    reader = comm.ExecuteReader()
                    Dim msOnDB As String
                    msOnDB = Nothing
                    If reader.HasRows = True Then
                        While reader.Read()
                            msOnDB = reader("MaTreEm")
                        End While
                    End If
                    If (msOnDB <> Nothing) Then
                        If (msOnDB.Length >= 6) Then
                            Dim currentYear = DateTime.Now.Year.ToString().Substring(2)
                            Dim iCurrentYear = Integer.Parse(currentYear)
                            Dim currentYearOnDB = msOnDB.Substring(0, 2)
                            Dim icurrentYearOnDB = Integer.Parse(currentYearOnDB)
                            Dim year = iCurrentYear
                            If (year < icurrentYearOnDB) Then
                                year = icurrentYearOnDB
                            End If
                            nextMste = year.ToString()
                            Dim v = msOnDB.Substring(2)
                            Dim convertDecimal = Convert.ToDecimal(v)
                            convertDecimal = convertDecimal + 1
                            Dim tmp = convertDecimal.ToString()
                            tmp = tmp.PadLeft(msOnDB.Length - 2, "0")
                            nextMste = nextMste + tmp
                            System.Console.WriteLine(nextMste)
                        End If
                    End If
                Catch ex As Exception
                    conn.Close() ' that bai!!!
                    System.Console.WriteLine(ex.StackTrace)
                    Return New Result(False, "Lấy tự động Mã số trẻ em kế tiếp không thành công", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True) ' thanh cong
    End Function
    Public Function insert(te As TreEmDTO) As Result

        Dim query As String = String.Empty
        query &= "INSERT INTO [tblTreEm] ([MaTreEm], [HoTenTreEm], [NgaySinh], [HoTenPhuHuynh], [TenONha], [DiaChi], [DienThoai],[Tuoi])"
        query &= "VALUES (@MaTreEm,@HoTenTreEm,@NgaySinh,@HoTenPhuHuynh,@TenONha,@DiaChi,@DienThoai,@Tuoi)"

        'get MSHS
        Dim nextMste = "1"
        buildMSTE(nextMste)
        te.StrMaTreEm1 = nextMste

        Using conn As New SqlConnection(connectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@MaTreEm", te.StrMaTreEm1)
                    .Parameters.AddWithValue("@HoTenTreEm", te.StrHoTenTreEm1)
                    .Parameters.AddWithValue("@NgaySinh", te.DateNgaySinh1)
                    .Parameters.AddWithValue("@HoTenPhuHuynh", te.StrHoTenPhuHuynh1)
                    .Parameters.AddWithValue("@TenONha", te.StrTenONha1)
                    .Parameters.AddWithValue("@DiaChi", te.StrDiaChi1)
                    .Parameters.AddWithValue("@DienThoai", te.StrDienThoai1)
                    .Parameters.AddWithValue("@Tuoi", te.IntTuoi1)
                End With
                Try
                    conn.Open()
                    comm.ExecuteNonQuery()
                Catch ex As Exception
                    conn.Close()
                    System.Console.WriteLine(ex.StackTrace)
                    Return New Result(False, "Thêm trẻ em không thành công", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True) ' thanh cong
    End Function
    Public Function selectALL(ByRef listTreEm As List(Of TreEmDTO)) As Result

        Dim query As String = String.Empty
        query &= "SELECT [MaTreEm], [HoTenTreEm], [NgaySinh], [HoTenPhuHuynh], [TenONha], [DiaChi], [DienThoai], [Tuoi]"
        query &= "FROM [tblTreEm]"

        Using conn As New SqlConnection(connectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                End With
                Try
                    conn.Open()
                    Dim reader As SqlDataReader
                    reader = comm.ExecuteReader()
                    listTreEm.Clear()
                    If reader.HasRows = True Then
                        While reader.Read()
                            listTreEm.Add(New TreEmDTO(reader("MaTreEm"), reader("HoTenTreEm"), reader("NgaySinh"), reader("HoTenPhuHuynh"), reader("TenONha"), reader("DiaChi"), reader("DienThoai"), reader("Tuoi")))
                        End While
                    End If

                Catch ex As Exception
                    conn.Close()
                    System.Console.WriteLine(ex.StackTrace)
                    Return New Result(False, "Lấy tất cả trẻ em không thành công", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True) ' thanh cong
    End Function
    Public Function selectByID(iMaTreEm As String, ByRef TreEmInfo As TreEmDTO) As Result

        Dim query As String = String.Empty
        query &= "SELECT [MaTreEm], [HoTenTreEm], [NgaySinh], [HoTenPhuHuynh], [TenONha], [DiaChi], [DienThoai], [Tuoi]"
        query &= "FROM [tblTreEm]"

        Using conn As New SqlConnection(connectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                End With
                Try
                    conn.Open()
                    Dim reader As SqlDataReader
                    reader = comm.ExecuteReader()
                    If reader.HasRows = True Then
                        While reader.Read()
                            If (reader("MaTreEm") = iMaTreEm) Then
                                TreEmInfo = (New TreEmDTO(reader("MaTreEm"), reader("HoTenTreEm"), reader("NgaySinh"), reader("HoTenPhuHuynh"), reader("TenONha"), reader("DiaChi"), reader("DienThoai"), reader("Tuoi")))
                            End If
                        End While
                    End If

                Catch ex As Exception
                    conn.Close()
                    System.Console.WriteLine(ex.StackTrace)
                    Return New Result(False, "Lấy tất cả trẻ em không thành công", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True) ' thanh cong
    End Function
    Public Function updatetByID(iMaTreEm As String, TreEmInfo As TreEmDTO) As Result
        Dim conn As New SqlConnection(connectionString)
        Try
            conn.Open()
            Dim txtMaSoTreEm As String = TreEmInfo.StrMaTreEm1
            Dim txtHoTen As String = TreEmInfo.StrHoTenTreEm1
            Dim txtNgaySinh As String = TreEmInfo.DateNgaySinh1
            Dim txtHoTenPhuHuynh As String = TreEmInfo.StrHoTenPhuHuynh1
            Dim txtTenONha As String = TreEmInfo.StrTenONha1
            Dim txtDiaChi As String = TreEmInfo.StrDiaChi1
            Dim txtDienThoai As String = TreEmInfo.StrDienThoai1
            Dim intTuoi As String = TreEmInfo.IntTuoi1

            Dim query As String = String.Empty
            query &= "UPDATE [tblTreEm] SET [HoTenTreEm] = '" + txtHoTen + "', [NgaySinh] = '" + txtNgaySinh + "', [HoTenPhuHuynh]='" + txtHoTenPhuHuynh + "', [TenONha]='" + txtTenONha + "', [DiaChi]='" + txtDiaChi + "', [DienThoai]='" + txtDienThoai + "', [Tuoi]='" + intTuoi + "' FROM [tblTreEm]"
            query &= "WHERE [MaTreEm]= '" + txtMaSoTreEm + "'"
            Dim comm As New SqlCommand(query, conn)
            comm.ExecuteNonQuery()
            conn.Close()
        Catch ex As Exception
            conn.Close()
            System.Console.WriteLine(ex.StackTrace)
            Return New Result(False, "Cập nhật trẻ em không thành công", ex.StackTrace)
        End Try


        Return New Result(True) ' thanh cong
    End Function
    Public Function deleteByID(iMaTreEm As String) As Result
        Dim conn As New SqlConnection(connectionString)
        Try
            conn.Open()
            Dim txtMaSoTreEm As String = iMaTreEm
            Dim query As String = String.Empty
            query &= "DELETE FROM [tblTreEm] WHERE [MaTreEm]= '" + txtMaSoTreEm + "'"
            Dim comm As New SqlCommand(query, conn)
            comm.ExecuteNonQuery()
            conn.Close()
        Catch ex As Exception
            conn.Close()
            System.Console.WriteLine(ex.StackTrace)
            Return New Result(False, "Cập nhật trẻ em không thành công", ex.StackTrace)
        End Try


        Return New Result(True) ' thanh cong
    End Function
    Public Function searchByText(searchText As String, ByRef listTreEm As List(Of TreEmDTO)) As Result
        Dim searchText1 As String = searchText
        Dim query As String = String.Empty
        query &= "SELECT * FROM [tblTreEm] WHERE CONCAT(HoTenTreEm,HoTenPhuHuynh) like '%" + searchText1 + "%'"

        Using conn As New SqlConnection(connectionString)
            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                End With
                Try
                    conn.Open()
                    Dim reader As SqlDataReader
                    reader = comm.ExecuteReader()
                    listTreEm.Clear()
                    If reader.HasRows = True Then
                        While reader.Read()
                            listTreEm.Add(New TreEmDTO(reader("MaTreEm"), reader("HoTenTreEm"), reader("NgaySinh"), reader("HoTenPhuHuynh"), reader("TenONha"), reader("DiaChi"), reader("DienThoai"), reader("Tuoi")))
                        End While
                    End If

                Catch ex As Exception
                    conn.Close()
                    System.Console.WriteLine(ex.StackTrace)
                    Return New Result(False, "Lấy tất cả trẻ em không thành công", ex.StackTrace)
                End Try
            End Using
        End Using
        Return New Result(True) ' thanh cong
    End Function
End Class