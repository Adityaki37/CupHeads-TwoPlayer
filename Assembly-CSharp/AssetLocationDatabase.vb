Imports System
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x020003A9 RID: 937
Public Class AssetLocationDatabase
	Inherits ScriptableObject

	' Token: 0x06000B92 RID: 2962 RVA: 0x00083FCC File Offset: 0x000823CC
	Public Sub SetDLCAssets(dlcAssets As String())
		Me.dlcAssetNames = dlcAssets
	End Sub

	' Token: 0x1700020B RID: 523
	' (get) Token: 0x06000B93 RID: 2963 RVA: 0x00083FD5 File Offset: 0x000823D5
	Public ReadOnly Property dlcAssets As HashSet(Of String)
		Get
			If Me._dlcAssets Is Nothing Then
				Me._dlcAssets = New HashSet(Of String)(Me.dlcAssetNames)
			End If
			Return Me._dlcAssets
		End Get
	End Property

	' Token: 0x04001520 RID: 5408
	<SerializeField()>
	Private dlcAssetNames As String()

	' Token: 0x04001521 RID: 5409
	Private _dlcAssets As HashSet(Of String)

	' Token: 0x020003AA RID: 938
	Public Enum AssetType
		' Token: 0x04001523 RID: 5411
		Base
		' Token: 0x04001524 RID: 5412
		DLC
	End Enum
End Class
