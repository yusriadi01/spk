Imports System.Data.Odbc

Public Class frm_login
    Dim sql As String = ""
    Public level As String = ""

    Public Sub bersih()
        txtUsername.Clear()
        txtPassword.Clear()
        txtUsername.Focus()
    End Sub

    Private Sub btnLogin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogin.Click
        sql = "select * from tbl_user where username=? and password=?"
        Dim cmd As New OdbcCommand(sql, koneksi)
        cmd.Parameters.Add("@username", OdbcType.VarChar).Value = txtUsername.Text
        cmd.Parameters.Add("@password", OdbcType.VarChar).Value = txtPassword.Text
        Dim dr As OdbcDataReader = cmd.ExecuteReader
        If dr.HasRows Then
            level = dr("level")
            frm_main.Show(Me)
            Me.Hide()
        Else
            MsgBox("Login Gagal")
        End If
    End Sub

    Private Sub frm_login_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        mulai_koneksi()
        bersih()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        End
    End Sub
End Class