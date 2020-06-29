Imports System.Data.Odbc

Public Class frm_user
    Dim sql As String = ""

    Private Sub frm_user_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        mulai_koneksi()
        set_tbl()
        cmblevel.SelectedIndex = 0
    End Sub

    Sub set_tbl()
        sql = "select * from v_user"
        Dim adapter As New OdbcDataAdapter(sql, koneksi)
        Dim ds As New DataSet
        adapter.Fill(ds)
        tbluser.DataSource = ds.Tables(0)
        With tbluser
            .Columns(0).Visible = False
            .AllowUserToAddRows = False            
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells            
        End With

        

        For i As Integer = 0 To tbluser.RowCount - 1
            Dim dt2 As New DataTable
            dt2.Columns.Add("nilai")
            dt2.Columns.Add("teks")

            dt2.Rows.Add("level 1", "Level 1")
            dt2.Rows.Add("level 2", "Level 2")

            Dim cmblvl As New DataGridViewComboBoxCell
            cmblvl.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing
            cmblvl.ValueMember = "nilai"
            cmblvl.DisplayMember = "teks"
            cmblvl.DataSource = dt2
            cmblvl.Value = tbluser.Item(2, i).Value
            tbluser.Item(2, i) = cmblvl
        Next

    End Sub

    Private Sub btnTambah_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTambah.Click
        txtUsername.Clear()
        txtPassword.Clear()
        txtnama.Clear()
        txtUsername.Focus()
    End Sub

    Private Sub btnSimpan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSimpan.Click
        If txtUsername.Text = "" And txtPassword.Text = "" Then Exit Sub
        sql = "insert into tbl_user(username,password,level,nama) values(?,?,?,?) on duplicate key update password=values(password), level=values(level), nama=values(nama)"
        Dim cmd As New OdbcCommand(sql, koneksi)
        cmd.Parameters.Add("@username", OdbcType.VarChar).Value = txtUsername.Text
        cmd.Parameters.Add("@password", OdbcType.VarChar).Value = txtPassword.Text
        cmd.Parameters.Add("@level", OdbcType.VarChar).Value = cmblevel.Text
        cmd.Parameters.Add("@nama", OdbcType.VarChar).Value = txtnama.Text

        Try
            cmd.ExecuteNonQuery()
            set_tbl()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub btnKeluar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnKeluar.Click
        Me.Close()
    End Sub

    Private Sub tbluser_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles tbluser.CellEndEdit
        sql = "update tbl_user set username=?, level=?, nama=? where id=?"
        Dim cmd As New OdbcCommand(sql, koneksi)
        cmd.Parameters.Add("@username", OdbcType.VarChar).Value = tbluser.Item(1, e.RowIndex).Value
        cmd.Parameters.Add("@level", OdbcType.VarChar).Value = tbluser.Item(2, e.RowIndex).Value
        cmd.Parameters.Add("@nama", OdbcType.VarChar).Value = tbluser.Item(3, e.RowIndex).Value
        cmd.Parameters.Add("@id", OdbcType.VarChar).Value = tbluser.Item(0, e.RowIndex).Value        
        
        Try
            cmd.ExecuteNonQuery()            
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub tbluser_UserDeletingRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowCancelEventArgs) Handles tbluser.UserDeletingRow
        sql = "delete from tbl_user where id=?"
        Dim cmd As New OdbcCommand(sql, koneksi)        
        cmd.Parameters.Add("@id", OdbcType.VarChar).Value = e.Row.Cells(0).Value

        Try
            cmd.ExecuteNonQuery()            
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class