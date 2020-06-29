Imports System.Data.Odbc

Module mdl_koneksi
    Public koneksi As New OdbcConnection("Driver={MySQL ODBC 3.51 Driver};Server=localhost;Database=db_ahp;User=root;Password=;Option=3;")

    Function cek_koneksi() As Boolean
        If koneksi.State = ConnectionState.Closed Then
            Try
                koneksi.Open()
                Return True
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
                Return False
            End Try
        End If
        Return True
    End Function

    Sub mulai_koneksi()
        If koneksi.State = ConnectionState.Closed Then
            Try
                koneksi.Open()
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
                End
            End Try
        End If
    End Sub

    Sub stop_koneksi()
        If koneksi.State = ConnectionState.Open Then
            Try
                koneksi.Close()
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
                End
            End Try
        End If
    End Sub

    Function escape_string(ByVal str As String) As String
        Dim filterStr As String = str
        Dim dangerChar() As String = {"'", """", "#"}
        For Each cr As String In dangerChar
            filterStr = Replace(filterStr, cr, "")
        Next
        Return filterStr
    End Function
End Module
