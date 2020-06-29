Imports System.Data.Odbc

Public Class frm_main
    Public tab_active = "kriteria"
    Dim sql As String = ""
    Dim kriteria = False, subkriteria = False, alternatif = False, penilaian = False, perbandinganberpasangan = False, ahp = False, laporan = False, tentang As Boolean = False

    Private Sub frm_main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        mulai_koneksi()
        If frm_login.level = "level 1" Then
            btnuser.Visible = True
        Else
            btnuser.Visible = False
        End If
    End Sub

    Sub set_btn(ByVal tambah As Boolean, ByVal simpan As Boolean, ByVal keluar As Boolean)
        btnTambah.Enabled = tambah
        btnSimpan.Enabled = simpan
        btnKeluar.Enabled = keluar
    End Sub

#Region "Kriteria"

    Sub set_tbl_kriteria()
        sql = "select * from v_kriteria order by id"
        Dim adapter As New OdbcDataAdapter(sql, koneksi)
        Dim dt As New DataTable
        adapter.Fill(dt)
        tblKriteria.DataSource = dt
        With tblKriteria
            .Columns(0).Visible = False
            .AllowUserToAddRows = False
            .Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        End With
    End Sub

    Private Sub tabKriteria_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabKriteria.Enter
        tab_active = "kriteria"
        If kriteria = False Then
            set_tbl_kriteria()
            kriteria = True
        End If
        set_btn(True, True, True)
    End Sub

    Private Sub tblKriteria_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles tblKriteria.CellEndEdit
        sql = "update tbl_kriteria set kriteria=?, peringkat=? where id=?"
        Dim cmd As New OdbcCommand(sql, koneksi)
        cmd.Parameters.Add("@kriteria", OdbcType.VarChar).Value = tblKriteria.Item("Nama Kriteria", e.RowIndex).Value
        cmd.Parameters.Add("@peringkat", OdbcType.Int).Value = tblKriteria.Item("Peringkat", e.RowIndex).Value
        cmd.Parameters.Add("@id", OdbcType.Int).Value = tblKriteria.Item("id", e.RowIndex).Value
        Try
            cmd.ExecuteNonQuery()            
        Catch ex As Exception
            MsgBox(ex.Message)
            set_tbl_kriteria()
        End Try
    End Sub

    Private Sub tblKriteria_UserDeletingRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowCancelEventArgs) Handles tblKriteria.UserDeletingRow
        sql = "delete from tbl_kriteria where id=?"
        Dim cmd As New OdbcCommand(sql, koneksi)
        cmd.Parameters.Add("@oldId", OdbcType.Int).Value = e.Row.Cells("id").Value
        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

#End Region

#Region "Subkriteria"

    Public Sub Populate(ByVal DataGridView As DataGridView, ByVal bs As BindingSource, ByVal bskriteria As BindingSource)
        Dim ds As New DataSet
        Dim da As New OdbcDataAdapter("select id as id_subkriteria,id_kriteria,subkriteria,skor from tbl_subkriteria order by id asc", koneksi)        
        da.Fill(ds, "tbl_subkriteria")
        da = New OdbcDataAdapter("select id as id_kriteria,kriteria from tbl_kriteria order by id asc", koneksi)
        da.Fill(ds, "tbl_kriteria")

        Dim idColumn As New DataGridViewTextBoxColumn
        With idColumn
            .DataPropertyName = "id_subkriteria"
            .HeaderText = "id"
        End With

        bskriteria.DataSource = ds.Tables("tbl_kriteria")

        Dim kriteria As New DataGridViewComboBoxColumn
        With kriteria
            .DataPropertyName = "id_kriteria"
            .DataSource = bskriteria
            .DisplayMember = "kriteria"
            .DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing
            .Name = "id_kriteria"
            .HeaderText = "Nama Kriteria"
            .SortMode = DataGridViewColumnSortMode.Automatic
            .ValueMember = "id_kriteria"
        End With

        Dim subkriteria As New DataGridViewTextBoxColumn
        With subkriteria
            .DataPropertyName = "subkriteria"
            .HeaderText = "Subkriteria"
        End With

        Dim skor As New DataGridViewTextBoxColumn
        With skor
            .DataPropertyName = "skor"
            .HeaderText = "Skor"
        End With

        DataGridView.AutoGenerateColumns = False
        DataGridView.Columns.Clear()
        DataGridView.Columns.AddRange(New DataGridViewColumn() {idColumn, kriteria, subkriteria, skor})

        bs.DataSource = ds.Tables("tbl_subkriteria")        
        DataGridView.DataSource = bs
        For Each col As DataGridViewColumn In DataGridView.Columns
            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        Next
    End Sub

    Sub set_cmb_kriteria()
        sql = "select id,kriteria from tbl_kriteria order by id asc"
        Dim adapter As New OdbcDataAdapter(sql, koneksi)
        Dim dt As New DataTable
        adapter.Fill(dt)
        cmbKriteria.ValueMember = "id"
        cmbKriteria.DisplayMember = "kriteria"
        cmbKriteria.DataSource = dt
    End Sub

    Sub set_tbl_subkriteria()
        Dim bskriteria As New BindingSource
        Dim bssubkriteria As New BindingSource        
        Populate(tblsubkriteria, bssubkriteria, bskriteria)
        With tblsubkriteria
            .Columns(0).Visible = False
            .AllowUserToAddRows = False
            .Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        End With
    End Sub

    Private Sub tabSubKriteria_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabSubkriteria.Enter
        tab_active = "subkriteria"
        If subkriteria = False Then
            set_tbl_subkriteria()
            set_cmb_kriteria()
            subkriteria = True
        End If
        set_btn(True, True, True)
    End Sub

    Private Sub tblSubkriteria_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles tblsubkriteria.CellEndEdit
        sql = "update tbl_subkriteria set id_kriteria=?, subkriteria=?, skor=? where id=?"
        Dim cmd As New OdbcCommand(sql, koneksi)
        cmd.Parameters.Add("@id_kriteria", OdbcType.VarChar).Value = tblsubkriteria.Item(1, e.RowIndex).Value
        cmd.Parameters.Add("@subkriteria", OdbcType.VarChar).Value = tblsubkriteria.Item(2, e.RowIndex).Value
        cmd.Parameters.Add("@skor", OdbcType.Int).Value = tblsubkriteria.Item(3, e.RowIndex).Value
        cmd.Parameters.Add("@id", OdbcType.Int).Value = tblsubkriteria.Item(0, e.RowIndex).Value
        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try        
    End Sub

    Private Sub tblsubkriteria_UserAddedRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowEventArgs) Handles tblsubkriteria.UserAddedRow                
    End Sub

    Private Sub tblSubkriteria_UserDeletingRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowCancelEventArgs) Handles tblsubkriteria.UserDeletingRow
        sql = "delete from tbl_subkriteria where id=?"
        Dim cmd As New OdbcCommand(sql, koneksi)
        cmd.Parameters.Add("@oldId", OdbcType.Int).Value = e.Row.Cells(0).Value
        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
#End Region

#Region "Alternatif"

    Sub set_tbl_alterntif()
        sql = "select * from v_alternatif order by id"
        Dim adapter As New OdbcDataAdapter(sql, koneksi)
        Dim dt As New DataTable
        adapter.Fill(dt)
        tblAlternatif.DataSource = dt
        With tblAlternatif
            .Columns(0).Visible = False
            .AllowUserToAddRows = False            
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        End With
    End Sub

    Private Sub tabAlternatif_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabAlternatif.Enter
        tab_active = "alternatif"
        If alternatif = False Then
            set_tbl_alterntif()
            alternatif = True
        End If
        set_btn(True, True, True)
    End Sub

    Private Sub tblAlternatif_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles tblAlternatif.CellEndEdit
        sql = "update tbl_alternatif set alternatif=?, keterangan=? where id=?"
        Dim cmd As New OdbcCommand(sql, koneksi)
        cmd.Parameters.Add("@alterantif", OdbcType.VarChar).Value = tblAlternatif.Item(1, e.RowIndex).Value
        cmd.Parameters.Add("@keterangan", OdbcType.VarChar).Value = tblAlternatif.Item(2, e.RowIndex).Value
        cmd.Parameters.Add("@id", OdbcType.Int).Value = tblAlternatif.Item(0, e.RowIndex).Value
        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.Message)
            set_tbl_kriteria()
        End Try
    End Sub

    Private Sub tblAlternatif_UserDeletingRow(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowCancelEventArgs) Handles tblAlternatif.UserDeletingRow
        sql = "delete from tbl_alternatif where id=?"
        Dim cmd As New OdbcCommand(sql, koneksi)
        cmd.Parameters.Add("@oldId", OdbcType.Int).Value = e.Row.Cells("id").Value
        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
#End Region

#Region "Penilaian Alternatif"

    Sub set_tbl_penilaian_alternatif()
        Cursor = Cursors.WaitCursor
        Dim ds As New DataSet
        Dim lstKriteria As New List(Of Integer)
        Dim lstNamaKriteria As New List(Of String)
        Dim lstBsKriteria As New List(Of BindingSource)
        Dim lstColKriteria As New List(Of DataGridViewComboBoxColumn)

        Dim idColumn As New DataGridViewTextBoxColumn
        With idColumn
            .DataPropertyName = "id"
            .HeaderText = "id"
            .Name = "id"
            .ReadOnly = True
            .SortMode = DataGridViewColumnSortMode.NotSortable
        End With

        Dim alternatifColumn As New DataGridViewTextBoxColumn
        With alternatifColumn
            .DataPropertyName = "alternatif"
            .HeaderText = "Alternatif"
            .Name = "alternatif"
            .SortMode = DataGridViewColumnSortMode.NotSortable
            .ReadOnly = True
        End With


        sql = "select id,kriteria from tbl_kriteria order by id asc"
        Dim cmd As New OdbcCommand(sql, koneksi)
        Dim dr As OdbcDataReader = cmd.ExecuteReader
        If dr.HasRows Then
            While dr.Read
                lstKriteria.Add(dr("id"))
                lstNamaKriteria.Add(dr("kriteria"))
            End While
        Else
            MsgBox("Tidak ada kriteria!", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
            TabControl1.SelectTab(0)
            dr.Close()
            Exit Sub
        End If
        dr.Close()

        For i As Integer = 0 To lstKriteria.Count - 1
            sql = "select id as id_subkriteria,subkriteria from tbl_subkriteria `" & lstKriteria(i).ToString & "` where id_kriteria='" & lstKriteria(i) & "'"
            Dim adapter As New OdbcDataAdapter(sql, koneksi)
            adapter.Fill(ds, lstKriteria(i).ToString)
        Next

        For i As Integer = 0 To ds.Tables.Count - 1
            Dim bs As New BindingSource
            bs.DataSource = ds.Tables(lstKriteria(i).ToString)
            lstBsKriteria.Add(bs)
        Next

        For i As Integer = 0 To lstBsKriteria.Count - 1
            Dim cmb As New DataGridViewComboBoxColumn
            With cmb
                .DataPropertyName = lstKriteria(i).ToString
                .DataSource = lstBsKriteria(i)
                .DisplayMember = "subkriteria"
                .DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing
                .Name = lstKriteria(i).ToString
                .HeaderText = lstNamaKriteria(i).ToString
                .SortMode = DataGridViewColumnSortMode.NotSortable
                .ValueMember = "id_subkriteria"
            End With
            lstColKriteria.Add(cmb)
        Next

        Dim dgvc As New List(Of DataGridViewColumn)
        dgvc.Add(idColumn)
        dgvc.Add(alternatifColumn)
        dgvc.AddRange(lstColKriteria.ToArray)

        With tblPenilaianAlternatif
            .AutoGenerateColumns = False
            .Columns.Clear()
            .Columns.AddRange(dgvc.ToArray)
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
        End With

        sql = "select id,alternatif from tbl_alternatif order by id asc"
        cmd = New OdbcCommand(sql, koneksi)
        dr = cmd.ExecuteReader

        Dim dt As New DataTable
        dt.Columns.Add("id", GetType(Integer))
        dt.Columns.Add("alternatif", GetType(String))

        For i As Integer = 0 To lstKriteria.Count - 1
            dt.Columns.Add(lstKriteria(i).ToString, GetType(Integer))
        Next

        If dr.HasRows Then
            While dr.Read
                dt.Rows.Add(dr("id"), dr("alternatif"))
            End While
        Else
            MsgBox("Tidak ada alternatif!", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
            TabControl1.SelectTab(2)
            dr.Close()
            Exit Sub
        End If

        Dim bs1 As New BindingSource
        bs1.DataSource = dt
        tblPenilaianAlternatif.DataSource = bs1

        For Each col As DataGridViewColumn In tblPenilaianAlternatif.Columns
            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        Next

        For i As Integer = 0 To tblPenilaianAlternatif.RowCount - 1
            For j As Integer = 2 To tblPenilaianAlternatif.ColumnCount - 1
                sql = "select id_subkriteria from tbl_penilaian where id_alternatif=? and id_kriteria=?"
                cmd = New OdbcCommand(sql, koneksi)
                cmd.Parameters.Add("@id_alternatif", OdbcType.Int).Value = tblPenilaianAlternatif.Item(0, i).Value
                cmd.Parameters.Add("@id_kriteria", OdbcType.Int).Value = Integer.Parse(tblPenilaianAlternatif.Columns(j).Name)
                dr = cmd.ExecuteReader
                If dr.HasRows Then
                    tblPenilaianAlternatif.Item(j, i).Value = dr("id_subkriteria")
                End If
            Next
        Next

        tblPenilaianAlternatif.Columns(0).Visible = False
        Cursor = Cursors.Default
    End Sub

    Private Sub tabPenilaianAlternatif_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabPenilaianAlternatif.Enter
        tab_active = "penilaian_alternatif"
        If penilaian = False Then
            set_tbl_penilaian_alternatif()
            penilaian = True
        End If
        set_btn(False, False, True)
    End Sub

    Private Sub tblPenilaianAlternatif_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles tblPenilaianAlternatif.CellEndEdit
        Cursor = Cursors.WaitCursor
        If Not IsDBNull(tblPenilaianAlternatif.Item(e.ColumnIndex, e.RowIndex).Value) Then
            sql = "insert into tbl_penilaian(id_alternatif,id_kriteria,id_subkriteria) values(?,?,?) on duplicate key update id_subkriteria=values(id_subkriteria)"
            Dim cmd As New OdbcCommand(sql, koneksi)
            cmd.Parameters.Add("@id_alternatif", OdbcType.Int).Value = Integer.Parse(tblPenilaianAlternatif.Item(0, e.RowIndex).Value)
            cmd.Parameters.Add("@id_kriteria", OdbcType.Int).Value = Integer.Parse(tblPenilaianAlternatif.Columns(e.ColumnIndex).Name)
            cmd.Parameters.Add("@id_subkriteria", OdbcType.Int).Value = Integer.Parse(tblPenilaianAlternatif.Item(e.ColumnIndex, e.RowIndex).Value)
            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
        Cursor = Cursors.Default
    End Sub

#End Region

#Region "Perbandingan Berpasangan"

    Private Sub tabPberpasangan_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabPberpasangan.Enter
        tab_active = "perbandinganberpasangan"
        If perbandinganberpasangan = False Then
            set_tbl_pb()
            set_cmb_kriteria_pb()
            perbandinganberpasangan = True
        End If
        set_btn(False, False, True)
    End Sub

    Function _RI(ByVal kriteria As Integer) As Double
        Select Case kriteria
            Case 1
                Return 0
            Case 2
                Return 0
            Case 3
                Return 0.58
            Case 4
                Return 0.9
            Case 5
                Return 1.12
            Case 6
                Return 1.24
            Case 7
                Return 1.32
            Case 8
                Return 1.41
            Case 9
                Return 1.45
            Case 10
                Return 1.49
            Case 11
                Return 1.51
            Case 12
                Return 1.48
            Case 13
                Return 1.56
            Case 14
                Return 1.57
            Case 15
                Return 1.59
            Case Else
                Return 0
        End Select
    End Function

    Function konsisten(ByVal cr As Double) As String
        If cr < 0.1 Then
            Return "Konsisten"
        Else
            Return "Tidak Konsisten"
        End If
    End Function

#Region "Kriteria"

    Sub set_tbl_pb()
        Cursor = Cursors.WaitCursor
        sql = "select id,kriteria from tbl_kriteria order by id asc"
        Dim cmd As New OdbcCommand(sql, koneksi)
        Dim dr As OdbcDataReader = cmd.ExecuteReader
        tblAturPb1.Columns.Clear()
        tblAturPb1.Rows.Clear()

        'untuk tbl aturpb2 atau tabel normalisasi
        tblAturPb2.Columns.Clear()
        tblAturPb2.Rows.Clear()

        tblAturPb1.Columns.Add("id", "id")

        Dim row As New DataGridViewRow
        row.HeaderCell.Value = "id"
        tblAturPb1.Rows.Add(row)

        If dr.HasRows Then
            While dr.Read
                tblAturPb1.Columns.Add(dr("id"), dr("kriteria"))
                tblAturPb2.Columns.Add(dr("id"), dr("kriteria"))

                row = New DataGridViewRow
                Dim row2 As New DataGridViewRow

                row.HeaderCell.Value = dr("kriteria")
                row2.HeaderCell.Value = dr("kriteria")
                tblAturPb1.Rows.Add(row)
                tblAturPb2.Rows.Add(row2)
            End While

            With tblAturPb1
                '.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader                
                .RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders
                .AllowUserToOrderColumns = False
                .AllowUserToAddRows = False
                .AllowUserToDeleteRows = False
                .SelectionMode = DataGridViewSelectionMode.CellSelect
            End With

            With tblAturPb2
                '.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader                
                .RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders
                .AllowUserToOrderColumns = False
                .AllowUserToAddRows = False
                .AllowUserToDeleteRows = False
                .SelectionMode = DataGridViewSelectionMode.CellSelect
                .EditMode = DataGridViewEditMode.EditProgrammatically
            End With

            For Each colom As DataGridViewColumn In tblAturPb1.Columns
                colom.SortMode = DataGridViewColumnSortMode.NotSortable
                colom.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                colom.Width = 100
            Next

            For Each colom As DataGridViewColumn In tblAturPb2.Columns
                colom.SortMode = DataGridViewColumnSortMode.NotSortable
                colom.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                colom.Width = 100
            Next

            cmd = New OdbcCommand(sql, koneksi)
            dr = cmd.ExecuteReader
            Dim noCol As Integer = 1
            While dr.Read
                tblAturPb1.Item(noCol, 0).Value = dr("id")
                tblAturPb1.Item(0, noCol).Value = dr("id")
                noCol += 1
            End While

            tblAturPb1.Columns(0).Visible = False
            tblAturPb1.Rows(0).Visible = False

            For i As Integer = 1 To tblAturPb1.RowCount - 1
                For j As Integer = 1 To tblAturPb1.ColumnCount - 1
                    If i = j Then
                        tblAturPb1.Item(j, i).Value = 1
                        tblAturPb1.Item(j, i).Style.BackColor = Color.LightGray
                        tblAturPb1.Item(j, i).ReadOnly = True
                    Else
                        sql = "select nilai from tbl_pbkriteria where id_kriteria=? and id_kriteria2=? limit 1"
                        cmd = New OdbcCommand(sql, koneksi)
                        cmd.Parameters.Add("@id_kriteria", OdbcType.Int).Value = tblAturPb1.Item(0, i).Value
                        cmd.Parameters.Add("@id_kriteria2", OdbcType.Int).Value = tblAturPb1.Item(j, 0).Value
                        dr = cmd.ExecuteReader

                        If dr.HasRows Then
                            tblAturPb1.Item(j, i).Value = dr("nilai")
                        Else
                            tblAturPb1.Item(j, i).Value = 1
                        End If
                    End If
                Next
            Next

            row = New DataGridViewRow
            row.HeaderCell.Value = "Jumlah"
            row.DefaultCellStyle.BackColor = Color.LightGray
            tblAturPb1.Rows.Add(row)

            For i As Integer = 1 To tblAturPb1.ColumnCount - 1
                Dim jlh As Double = 0
                For j As Integer = 1 To tblAturPb1.RowCount - 1
                    jlh += tblAturPb1.Item(i, j).Value
                Next
                tblAturPb1.Item(i, tblAturPb1.RowCount - 1).Value = jlh
            Next

            atur_tbl_normalisasi()

            'untuk tblaturpb3 atau tabel penjumlahan tiap baris
            With tblAturPb3
                .Columns.Clear()
                .Rows.Clear()

                .Columns.Add("jumlah", "Jumlah")
                .Columns.Add("pv", "PV")
                .Columns.Add("jlh_pv", "€ PV")
                .Rows.Add(tblAturPb2.RowCount)
                .RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders
                .AllowUserToOrderColumns = False
                .AllowUserToAddRows = False
                .AllowUserToDeleteRows = False
                .SelectionMode = DataGridViewSelectionMode.CellSelect
                .EditMode = DataGridViewEditMode.EditProgrammatically
            End With

            For Each colom As DataGridViewColumn In tblAturPb3.Columns
                colom.SortMode = DataGridViewColumnSortMode.NotSortable
                colom.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                colom.Width = 100
            Next

            atur_tbl_jumlah_tiap_baris()
            atur_max()
        Else
            MsgBox("Kriteria tidak ada!", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
            TabControl1.SelectTab(0)
        End If
        dr.Close()
        Cursor = Cursors.Default
    End Sub

    Sub atur_tbl_normalisasi()
        For i As Integer = 0 To tblAturPb2.ColumnCount - 1
            For j As Integer = 0 To tblAturPb2.RowCount - 1
                tblAturPb2.Item(i, j).Value = tblAturPb1.Item(i + 1, j + 1).Value / tblAturPb1.Item(i + 1, tblAturPb1.RowCount - 1).Value
            Next
        Next
    End Sub

    Sub atur_tbl_jumlah_tiap_baris()
        'kolom jumlah        
        Dim jlhKriteria As Integer = tblAturPb3.RowCount
        For j As Integer = 0 To tblAturPb2.RowCount - 1
            Dim jlh As Double = 0
            For k As Integer = 0 To tblAturPb2.ColumnCount - 1
                jlh += tblAturPb2.Item(k, j).Value
            Next
            tblAturPb3.Item(0, j).Value = jlh
            simpan_hasil_kriteria(tblAturPb1.Item(0, j + 1).Value, jlh / jlhKriteria)
        Next

        For i As Integer = 0 To tblAturPb3.RowCount - 1
            tblAturPb3.Item(1, i).Value = tblAturPb3.Item(0, i).Value / jlhKriteria
            Dim jlhPv As Double = 0
            For j As Integer = 0 To tblAturPb1.ColumnCount - 2
                jlhPv += tblAturPb1.Item(j + 1, i + 1).Value * tblAturPb3.Item(1, j).Value
            Next
            tblAturPb3.Item(2, i).Value = jlhPv
        Next

        For i As Integer = 0 To tblAturPb3.RowCount - 1            
            Dim jlhPv As Double = 0
            For j As Integer = 0 To tblAturPb1.ColumnCount - 2
                jlhPv += tblAturPb1.Item(j + 1, i + 1).Value * tblAturPb3.Item(1, j).Value
            Next
            tblAturPb3.Item(2, i).Value = jlhPv
        Next
    End Sub

    Sub atur_max()
        Dim jlhKriteria As Integer = tblAturPb3.RowCount
        Dim nmax As Double = 0

        For i As Integer = 0 To tblAturPb3.RowCount - 1
            nmax += tblAturPb3.Item(2, i).Value / tblAturPb3.Item(1, i).Value
        Next
        nmax = nmax / jlhKriteria
        lblMaks.Text = nmax

        Dim ci As Double = (nmax - jlhKriteria) / (jlhKriteria - 1)
        lblCI.Text = ci

        Dim ri As Double = _RI(jlhKriteria)
        lblRI.Text = ri

        Dim cr As Double = ci / ri
        lblCR.Text = cr

        lblCR.Text &= vbNewLine & konsisten(cr)
    End Sub

    Sub simpan_hasil_kriteria(ByVal id_kriteria As Integer, ByVal hasil As Double)        
        sql = "insert into tbl_hasilkriteria(id_kriteria,hasil) values(?,?) on duplicate key update hasil=values(hasil)"
        Dim cmd As New OdbcCommand(sql, koneksi)
        cmd.Parameters.Add("@id_kriteria", OdbcType.Int).Value = id_kriteria
        cmd.Parameters.Add("@hasil", OdbcType.Double).Value = hasil
        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub tblAturPb1_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles tblAturPb1.CellEndEdit
        Cursor = Cursors.WaitCursor
        tblAturPb1.Item(e.RowIndex, e.ColumnIndex).Value = 1 / tblAturPb1.Item(e.ColumnIndex, e.RowIndex).Value

        Dim jlh_base_row As Double = 0
        Dim jlh_base_col As Double = 0
        For i As Integer = 1 To tblAturPb1.RowCount - 2
            jlh_base_row += tblAturPb1.Item(e.RowIndex, i).Value
            jlh_base_col += tblAturPb1.Item(e.ColumnIndex, i).Value
        Next
        tblAturPb1.Item(e.RowIndex, tblAturPb1.RowCount - 1).Value = jlh_base_row
        tblAturPb1.Item(e.ColumnIndex, tblAturPb1.RowCount - 1).Value = jlh_base_col

        sql = "insert into tbl_pbkriteria(id_kriteria,id_kriteria2,nilai) values(?,?,?) on duplicate key update nilai=values(nilai)"
        Dim cmd As New OdbcCommand(sql, koneksi)
        cmd.Parameters.Add("@id_kriteria", OdbcType.Int).Value = tblAturPb1.Item(0, e.RowIndex).Value
        cmd.Parameters.Add("@id_kriteria2", OdbcType.Int).Value = tblAturPb1.Item(e.ColumnIndex, 0).Value
        cmd.Parameters.Add("@nilai", OdbcType.Double).Value = tblAturPb1.Item(e.ColumnIndex, e.RowIndex).Value
        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        cmd = New OdbcCommand(sql, koneksi)
        cmd.Parameters.Add("@id_kriteria", OdbcType.Int).Value = tblAturPb1.Item(0, e.ColumnIndex).Value
        cmd.Parameters.Add("@id_kriteria2", OdbcType.Int).Value = tblAturPb1.Item(e.RowIndex, 0).Value
        cmd.Parameters.Add("@nilai", OdbcType.Double).Value = tblAturPb1.Item(e.RowIndex, e.ColumnIndex).Value
        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        atur_tbl_normalisasi()
        atur_tbl_jumlah_tiap_baris()
        atur_max()
        Cursor = Cursors.Default
    End Sub

#End Region

#Region "Alterntif"
    Sub set_cmb_kriteria_pb()
        Cursor = Cursors.WaitCursor
        sql = "select id,kriteria from tbl_kriteria order by id asc"
        Dim adapter As New OdbcDataAdapter(sql, koneksi)
        Dim dt As New DataTable
        adapter.Fill(dt)
        cmbkriteriaspb.ValueMember = "id"
        cmbkriteriaspb.DisplayMember = "kriteria"
        cmbkriteriaspb.DataSource = dt
        Cursor = Cursors.Default
    End Sub

    Sub set_tbl_pb_alternatif()
        Cursor = Cursors.WaitCursor
        sql = "select id,alternatif from tbl_alternatif order by id asc"
        Dim cmd As New OdbcCommand(sql, koneksi)
        Dim dr As OdbcDataReader = cmd.ExecuteReader
        tblAturPbAlternatif1.Columns.Clear()
        tblAturPbAlternatif1.Rows.Clear()

        'untuk tbl aturpb2 atau tabel normalisasi
        tblAturPbAlternatif2.Columns.Clear()
        tblAturPbAlternatif2.Rows.Clear()

        tblAturPbAlternatif1.Columns.Add("id", "id")

        Dim row As New DataGridViewRow
        row.HeaderCell.Value = "id"
        tblAturPbAlternatif1.Rows.Add(row)

        If dr.HasRows Then
            While dr.Read
                tblAturPbAlternatif1.Columns.Add(dr("id"), dr("alternatif"))
                tblAturPbAlternatif2.Columns.Add(dr("id"), dr("alternatif"))

                row = New DataGridViewRow
                Dim row2 As New DataGridViewRow

                row.HeaderCell.Value = dr("alternatif")
                row2.HeaderCell.Value = dr("alternatif")
                tblAturPbAlternatif1.Rows.Add(row)
                tblAturPbAlternatif2.Rows.Add(row2)
            End While

            With tblAturPbAlternatif1
                '.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader                
                .RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders
                .AllowUserToOrderColumns = False
                .AllowUserToAddRows = False
                .AllowUserToDeleteRows = False
                .SelectionMode = DataGridViewSelectionMode.CellSelect
                .EditMode = DataGridViewEditMode.EditProgrammatically
            End With

            With tblAturPbAlternatif2
                '.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader                
                .RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders
                .AllowUserToOrderColumns = False
                .AllowUserToAddRows = False
                .AllowUserToDeleteRows = False
                .SelectionMode = DataGridViewSelectionMode.CellSelect
                .EditMode = DataGridViewEditMode.EditProgrammatically
            End With

            For Each colom As DataGridViewColumn In tblAturPbAlternatif1.Columns
                colom.SortMode = DataGridViewColumnSortMode.NotSortable
                colom.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                colom.Width = 100
            Next

            For Each colom As DataGridViewColumn In tblAturPbAlternatif2.Columns
                colom.SortMode = DataGridViewColumnSortMode.NotSortable
                colom.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                colom.Width = 100
            Next

            cmd = New OdbcCommand(sql, koneksi)
            dr = cmd.ExecuteReader
            Dim noCol As Integer = 1
            While dr.Read
                tblAturPbAlternatif1.Item(noCol, 0).Value = dr("id")
                tblAturPbAlternatif1.Item(0, noCol).Value = dr("id")
                noCol += 1
            End While

            tblAturPbAlternatif1.Columns(0).Visible = False
            tblAturPbAlternatif1.Rows(0).Visible = False

            For i As Integer = 1 To tblAturPbAlternatif1.ColumnCount - 1
                Dim nilaiCol As Integer = 1

                sql = "SELECT tbl_subkriteria.skor FROM tbl_subkriteria Inner Join tbl_penilaian ON tbl_penilaian.id_subkriteria = tbl_subkriteria.id WHERE tbl_penilaian.id_kriteria =  ? AND tbl_penilaian.id_alternatif =  ? limit 1"
                cmd = New OdbcCommand(sql, koneksi)
                cmd.Parameters.Add("@id_kriteria", OdbcType.Int).Value = cmbkriteriaspb.SelectedValue
                cmd.Parameters.Add("@id_alternaif", OdbcType.Int).Value = tblAturPbAlternatif1.Item(i, 0).Value
                dr = cmd.ExecuteReader
                If dr.HasRows Then
                    nilaiCol = dr("skor")
                End If

                For j As Integer = 1 To tblAturPbAlternatif1.RowCount - 1
                    If i = j Then
                        tblAturPbAlternatif1.Item(i, j).Value = 1
                        tblAturPbAlternatif1.Item(i, j).Style.BackColor = Color.LightGray
                        tblAturPbAlternatif1.Item(i, j).ReadOnly = True
                    Else
                        Dim nilaiRow As Integer = 1
                        cmd = New OdbcCommand(sql, koneksi)
                        cmd.Parameters.Add("@id_kriteria", OdbcType.Int).Value = cmbkriteriaspb.SelectedValue
                        cmd.Parameters.Add("@id_alternaif", OdbcType.Int).Value = tblAturPbAlternatif1.Item(0, j).Value
                        dr = cmd.ExecuteReader
                        If dr.HasRows Then
                            nilaiRow = dr("skor")
                        End If

                        tblAturPbAlternatif1.Item(i, j).Value = nilaiRow / nilaiCol
                    End If
                Next
            Next

            row = New DataGridViewRow
            row.HeaderCell.Value = "Jumlah"
            row.DefaultCellStyle.BackColor = Color.LightGray
            tblAturPbAlternatif1.Rows.Add(row)

            For i As Integer = 1 To tblAturPbAlternatif1.ColumnCount - 1
                Dim jlh As Double = 0
                For j As Integer = 1 To tblAturPbAlternatif1.RowCount - 1
                    jlh += tblAturPbAlternatif1.Item(i, j).Value
                Next
                tblAturPbAlternatif1.Item(i, tblAturPbAlternatif1.RowCount - 1).Value = jlh
            Next

            atur_tbl_normalisasi_alternatif()

            'untuk tblaturpb3 atau tabel penjumlahan tiap baris
            With tblAturPbAlternatif3
                .Columns.Clear()
                .Rows.Clear()

                .Columns.Add("jumlah", "Jumlah")
                .Columns.Add("pv", "PV")
                .Columns.Add("jlh_pv", "€ PV")
                .Rows.Add(tblAturPbAlternatif2.RowCount)
                .RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders
                .AllowUserToOrderColumns = False
                .AllowUserToAddRows = False
                .AllowUserToDeleteRows = False
                .SelectionMode = DataGridViewSelectionMode.CellSelect
                .EditMode = DataGridViewEditMode.EditProgrammatically
            End With

            For Each colom As DataGridViewColumn In tblAturPbAlternatif3.Columns
                colom.SortMode = DataGridViewColumnSortMode.NotSortable
                colom.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                colom.Width = 100
            Next
            atur_tbl_jumlah_tiap_baris_alternatif()
            atur_max_alternatif()
        Else
            MsgBox("Alternatif tidak ada!", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
            TabControl1.SelectTab(2)
        End If
        dr.Close()
        Cursor = Cursors.Default
    End Sub

    Sub atur_tbl_normalisasi_alternatif()
        For i As Integer = 0 To tblAturPbAlternatif2.ColumnCount - 1
            For j As Integer = 0 To tblAturPbAlternatif2.RowCount - 1
                tblAturPbAlternatif2.Item(i, j).Value = tblAturPbAlternatif1.Item(i + 1, j + 1).Value / tblAturPbAlternatif1.Item(i + 1, tblAturPbAlternatif1.RowCount - 1).Value
            Next
        Next
    End Sub

    Sub atur_tbl_jumlah_tiap_baris_alternatif()
        'kolom jumlah        
        Dim jlhAlternatif As Integer = tblAturPbAlternatif3.RowCount
        For j As Integer = 0 To tblAturPbAlternatif2.RowCount - 1
            Dim jlh As Double = 0
            For k As Integer = 0 To tblAturPbAlternatif2.ColumnCount - 1
                jlh += tblAturPbAlternatif2.Item(k, j).Value
            Next
            tblAturPbAlternatif3.Item(0, j).Value = jlh
            simpan_hasil_alternatif(cmbkriteriaspb.SelectedValue, tblAturPbAlternatif1.Item(0, j + 1).Value, jlh / jlhAlternatif)
        Next

        For i As Integer = 0 To tblAturPbAlternatif3.RowCount - 1
            tblAturPbAlternatif3.Item(1, i).Value = tblAturPbAlternatif3.Item(0, i).Value / jlhAlternatif
            Dim jlhPv As Double = 0
            For j As Integer = 0 To tblAturPbAlternatif1.ColumnCount - 2
                jlhPv += tblAturPbAlternatif1.Item(j + 1, i + 1).Value * tblAturPbAlternatif3.Item(1, j).Value
            Next
            tblAturPbAlternatif3.Item(2, i).Value = jlhPv
        Next

        For i As Integer = 0 To tblAturPbAlternatif3.RowCount - 1
            Dim jlhPv As Double = 0
            For j As Integer = 0 To tblAturPbAlternatif1.ColumnCount - 2
                jlhPv += tblAturPbAlternatif1.Item(j + 1, i + 1).Value * tblAturPbAlternatif3.Item(1, j).Value
            Next
            tblAturPbAlternatif3.Item(2, i).Value = jlhPv
        Next
    End Sub

    Sub atur_max_alternatif()
        Dim jlhKriteria As Integer = tblAturPbAlternatif3.RowCount
        Dim nmax As Double = 0

        For i As Integer = 0 To tblAturPbAlternatif3.RowCount - 1
            nmax += tblAturPbAlternatif3.Item(2, i).Value / tblAturPbAlternatif3.Item(1, i).Value
        Next
        nmax = nmax / jlhKriteria
        Label15.Text = nmax

        Dim ci As Double = (nmax - jlhKriteria) / (jlhKriteria - 1)
        Label14.Text = ci

        Dim ri As Double = _RI(jlhKriteria)
        Label13.Text = ri

        Dim cr As Double = ci / ri
        Label12.Text = cr

        Label12.Text &= vbNewLine & konsisten(cr)
    End Sub

    Sub simpan_hasil_alternatif(ByVal id_kriteria As Integer, ByVal id_alternatif As Integer, ByVal hasil As Double)
        sql = "insert into tbl_hasilalternatif(id_kriteria,id_alternatif,hasil) values(?,?,?) on duplicate key update hasil=values(hasil)"
        Dim cmd As New OdbcCommand(sql, koneksi)
        cmd.Parameters.Add("@id_kriteria", OdbcType.Int).Value = id_kriteria
        cmd.Parameters.Add("@id_alternatif", OdbcType.Int).Value = id_alternatif
        cmd.Parameters.Add("@hasil", OdbcType.Double).Value = hasil
        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub tblAturPbAlternatif_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles tblAturPbAlternatif1.CellEndEdit
        Cursor = Cursors.WaitCursor
        tblAturPbAlternatif1.Item(e.RowIndex, e.ColumnIndex).Value = 1 / tblAturPbAlternatif1.Item(e.ColumnIndex, e.RowIndex).Value

        Dim jlh_base_row As Double = 0
        Dim jlh_base_col As Double = 0
        For i As Integer = 1 To tblAturPbAlternatif1.RowCount - 2
            jlh_base_row += tblAturPbAlternatif1.Item(e.RowIndex, i).Value
            jlh_base_col += tblAturPbAlternatif1.Item(e.ColumnIndex, i).Value
        Next
        tblAturPbAlternatif1.Item(e.RowIndex, tblAturPbAlternatif1.RowCount - 1).Value = jlh_base_row
        tblAturPbAlternatif1.Item(e.ColumnIndex, tblAturPbAlternatif1.RowCount - 1).Value = jlh_base_col

        sql = "insert into tbl_pbalternatif(id_kriteria,id_alternatif,id_alternatif2,nilai) values(?,?,?,?) on duplicate key update nilai=values(nilai)"
        Dim cmd As New OdbcCommand(sql, koneksi)
        cmd.Parameters.Add("@id_kriteria", OdbcType.Int).Value = cmbkriteriaspb.SelectedValue
        cmd.Parameters.Add("@id_alternatif", OdbcType.Int).Value = tblAturPbAlternatif1.Item(0, e.RowIndex).Value
        cmd.Parameters.Add("@id_alternatif2", OdbcType.Int).Value = tblAturPbAlternatif1.Item(e.ColumnIndex, 0).Value
        cmd.Parameters.Add("@nilai", OdbcType.Double).Value = tblAturPbAlternatif1.Item(e.ColumnIndex, e.RowIndex).Value
        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        cmd = New OdbcCommand(sql, koneksi)
        cmd.Parameters.Add("@id_kriteria", OdbcType.Int).Value = cmbkriteriaspb.SelectedValue
        cmd.Parameters.Add("@id_alternatif", OdbcType.Int).Value = tblAturPbAlternatif1.Item(0, e.ColumnIndex).Value
        cmd.Parameters.Add("@id_alternatif2", OdbcType.Int).Value = tblAturPbAlternatif1.Item(e.RowIndex, 0).Value
        cmd.Parameters.Add("@nilai", OdbcType.Double).Value = tblAturPbAlternatif1.Item(e.RowIndex, e.ColumnIndex).Value
        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        atur_tbl_normalisasi_alternatif()
        atur_tbl_jumlah_tiap_baris_alternatif()
        atur_max_alternatif()
        Cursor = Cursors.Default
    End Sub

    Private Sub cmbkriteriaspb_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbkriteriaspb.SelectedIndexChanged
        set_tbl_pb_alternatif()
    End Sub
#End Region

#End Region

#Region "Ahp"

    Sub set_tbl_ahp(Optional ByVal rangking As Integer = Nothing, Optional ByVal nilai As Double = Nothing)
        sql = "select id_alternatif,(@no:=@no+1) as Rangking,Alternatif,AHP,Keterangan from v_hasil,(select @no:=0) t order by `AHP` DESC"
        If Not rangking = Nothing Then
            sql = "select id_alternatif,(@no:=@no+1) as Rangking,Alternatif,AHP,Keterangan from v_hasil,(select @no:=0) t order by `AHP` DESC limit " & escape_string(rangking)
        End If

        If Not nilai = Nothing Then
            sql = "select id_alternatif,(@no:=@no+1) as Rangking,Alternatif,AHP,Keterangan from v_hasil,(select @no:=0) t where `AHP` >= " & nilai & " order by `AHP` DESC"
        End If

        Dim adapter As New OdbcDataAdapter(sql, koneksi)
        Dim dt As New DataTable
        adapter.Fill(dt)

        tblAhp.DataSource = dt

        With tblAhp
            .Columns(0).Visible = False
            .Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopCenter
            .EditMode = DataGridViewEditMode.EditProgrammatically
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
        End With

        For Each column As DataGridViewColumn In tblAhp.Columns
            column.SortMode = DataGridViewColumnSortMode.NotSortable
        Next
    End Sub

    Private Sub tabAhp_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabAhp.Enter
        tab_active = "ahp"
        If ahp = False Then
            set_tbl_ahp()
            ahp = True
        End If
        set_btn(False, False, True)
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        If txtFilter.Text = "" Then
            txtFilter.Text = "0"
        End If

        If RadioButton1.Checked Then
            set_tbl_ahp()
        End If

        If RadioButton2.Checked Then
            set_tbl_ahp(Integer.Parse(txtFilter.Text))
        End If

        If RadioButton3.Checked Then
            set_tbl_ahp(Nothing, Double.Parse(txtFilter.Text))
        End If
    End Sub

    Private Sub txtFilter_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtFilter.KeyPress
        If Not RadioButton3.Checked Then
            If Not IsNumeric(e.KeyChar) And Not e.KeyChar = vbBack Then
                e.KeyChar = Nothing
            End If
        Else
            If Not IsNumeric(e.KeyChar) And Not e.KeyChar = vbBack And Not e.KeyChar = "." Then
                e.KeyChar = Nothing
            End If
        End If
    End Sub

    Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton1.CheckedChanged
        txtFilter.Clear()
    End Sub

    Private Sub RadioButton2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton2.CheckedChanged
        txtFilter.Clear()
        txtFilter.Focus()
    End Sub

    Private Sub RadioButton3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton3.CheckedChanged
        txtFilter.Clear()
        txtFilter.Focus()
    End Sub
#End Region

#Region "Laporan"
    Private Sub LaporanKriteria_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton4.CheckedChanged
        If RadioButton4.Checked Then
            cr.ReportSource = New rpt_kriteria            
        End If
    End Sub

    Private Sub LaporanSubKriteria_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton5.CheckedChanged
        If RadioButton5.Checked Then
            cr.ReportSource = New rpt_subkriteria            
        End If
    End Sub

    Private Sub LaporanPb_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton6.CheckedChanged
        If RadioButton6.Checked Then
            cr.ReportSource = New rpt_pbkriteria            
        End If
    End Sub

    Private Sub LaporanAlternatif_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton7.CheckedChanged
        If RadioButton7.Checked Then
            cr.ReportSource = New rpt_alternatif            
        End If
    End Sub

    Private Sub LaporanPenilaian_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton8.CheckedChanged
        If RadioButton8.Checked Then
            cr.ReportSource = New rpt_penilaian            
        End If
    End Sub

    Private Sub LaporanAHP_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton9.CheckedChanged
        If RadioButton9.Checked Then
            GroupBox7.Visible = True
            If RadioButton12.Checked = True Then
                cr.ReportSource = New rpt_ahp
            Else
                RadioButton12.Checked = True
            End If
        Else
            GroupBox7.Visible = False
        End If
    End Sub

    Private Sub LaporanAHPsemua_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton12.CheckedChanged
        If RadioButton12.Checked Then
            cr.ReportSource = New rpt_ahp
        End If
    End Sub

    Private Sub TextBox1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1.KeyPress
        If Not RadioButton10.Checked Then
            If Not IsNumeric(e.KeyChar) And Not e.KeyChar = vbBack Then
                e.KeyChar = Nothing
            End If
        Else
            If Not IsNumeric(e.KeyChar) And Not e.KeyChar = vbBack And Not e.KeyChar = "." Then
                e.KeyChar = Nothing
            End If
        End If
    End Sub

    Private Sub RadioButton11_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton10.CheckedChanged
        TextBox1.Text = 0
        TextBox1.Focus()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        
        If RadioButton10.Checked Then
            cr.SelectionFormula = "{v_hasil.AHP}>=" & Val(TextBox1.Text)
            cr.RefreshReport()
        End If
    End Sub

    Private Sub tabLaporan_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabLaporan.Enter
        tab_active = "laporan"
        set_btn(False, False, True)
    End Sub

#End Region

    Private Sub btnSimpan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSimpan.Click
        If tab_active = "kriteria" Then
            sql = "insert into tbl_kriteria(kriteria,peringkat) values(?,?)"
            Dim cmd As New OdbcCommand(sql, koneksi)
            cmd.Parameters.Add("@kriteria", OdbcType.VarChar).Value = txtKriteria.Text
            cmd.Parameters.Add("@peringkat", OdbcType.Int).Value = numPeringkat.Value
            Try
                cmd.ExecuteNonQuery()
                set_tbl_kriteria()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        ElseIf tab_active = "subkriteria" Then
            sql = "insert into tbl_subkriteria(id_kriteria,subkriteria,skor) values(?,?,?)"
            Dim cmd As New OdbcCommand(sql, koneksi)
            cmd.Parameters.Add("@id_kriteria", OdbcType.VarChar).Value = cmbKriteria.SelectedValue
            cmd.Parameters.Add("@subkriteria", OdbcType.VarChar).Value = txtSubKriteria.Text
            cmd.Parameters.Add("@skor", OdbcType.Int).Value = numSkor.Value
            Try
                cmd.ExecuteNonQuery()
                set_tbl_subkriteria()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        ElseIf tab_active = "alternatif" Then
            sql = "insert into tbl_alternatif(alternatif,keterangan) values(?,?)"
            Dim cmd As New OdbcCommand(sql, koneksi)
            cmd.Parameters.Add("@alternati", OdbcType.VarChar).Value = txtAlternatif.Text
            cmd.Parameters.Add("@keterangan", OdbcType.VarChar).Value = txtKeteranganAlternatif.Text
            Try
                cmd.ExecuteNonQuery()
                set_tbl_alterntif()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub

    Private Sub btnTambah_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTambah.Click
        If tab_active = "kriteria" Then
            txtKriteria.Clear()
            numPeringkat.Value = 0
            txtKriteria.Focus()
        ElseIf tab_active = "subkriteria" Then
            On Error Resume Next
            cmbKriteria.SelectedValue = 0
            txtSubKriteria.Clear()
            numSkor.Value = 0
            txtSubKriteria.Focus()
        ElseIf tab_active = "alternatif" Then
            On Error Resume Next
            txtAlternatif.Clear()
            txtKeteranganAlternatif.Clear()
            txtAlternatif.Focus()
        End If
    End Sub

    Private Sub btnRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
        If tab_active = "kriteria" Then
            set_tbl_kriteria()
        ElseIf tab_active = "subkriteria" Then
            set_tbl_subkriteria()
            set_cmb_kriteria()
        ElseIf tab_active = "alternatif" Then
            set_tbl_alterntif()
        ElseIf tab_active = "penilaian_alternatif" Then
            set_tbl_penilaian_alternatif()
        ElseIf tab_active = "perbandinganberpasangan" Then
            set_tbl_pb()
            set_cmb_kriteria_pb()
        ElseIf tab_active = "ahp" Then
            Button5_Click(sender, e)
        End If
    End Sub

    Private Sub btnuser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnuser.Click
        frm_user.ShowDialog(Me)
    End Sub

    Private Sub btnKeluar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnKeluar.Click
        frm_login.bersih()
        frm_login.Show()
        Me.Close()
    End Sub

End Class