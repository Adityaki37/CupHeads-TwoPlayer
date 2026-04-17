Imports System
Imports System.Collections
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x020008A5 RID: 2213
Public Class CircusPlatformingLevelHotdogProjectile
	Inherits BasicProjectile

	' Token: 0x14000061 RID: 97
	' (add) Token: 0x06003384 RID: 13188 RVA: 0x001DF3AC File Offset: 0x001DD7AC
	' (remove) Token: 0x06003385 RID: 13189 RVA: 0x001DF3E4 File Offset: 0x001DD7E4
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnDestroyCallback As Action(Of CircusPlatformingLevelHotdogProjectile)

	' Token: 0x17000445 RID: 1093
	' (get) Token: 0x06003386 RID: 13190 RVA: 0x001DF41A File Offset: 0x001DD81A
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return 20F
		End Get
	End Property

	' Token: 0x06003387 RID: 13191 RVA: 0x001DF421 File Offset: 0x001DD821
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.collider2d = MyBase.GetComponent(Of Collider2D)()
	End Sub

	' Token: 0x06003388 RID: 13192 RVA: 0x001DF438 File Offset: 0x001DD838
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.transform.localScale = New Vector3(0F, 1F, 1F)
		Me.spark.Create(MyBase.transform.position - New Vector3(10F, 0F, 0F))
		MyBase.StartCoroutine(Me.scaleOnStart_cr())
	End Sub

	' Token: 0x06003389 RID: 13193 RVA: 0x001DF4A8 File Offset: 0x001DD8A8
	Private Iterator Function scaleOnStart_cr() As IEnumerator
		While MyBase.transform.localScale.x < 1F
			MyBase.transform.AddScale(Me.scaleFactor * CupheadTime.Delta, 0F, 0F)
			Yield Nothing
		End While
		MyBase.transform.SetScale(New Single?(1F), New Single?(1F), New Single?(1F))
		Return
	End Function

	' Token: 0x0600338A RID: 13194 RVA: 0x001DF4C4 File Offset: 0x001DD8C4
	Public Sub Side(isRight As Boolean)
		If isRight Then
			For i As Integer = 0 To Me.renderers.Length - 1
				Me.renderers(i).sortingOrder += 3
			Next
		End If
	End Sub

	' Token: 0x0600338B RID: 13195 RVA: 0x001DF508 File Offset: 0x001DD908
	Public Sub SetCondiment(type As String)
		If type = "K" Then
			MyBase.animator.Play("Ketchup")
		ElseIf type = "M" Then
			MyBase.animator.Play("Mustard")
		ElseIf type = "R" Then
			MyBase.animator.Play("Relish")
		End If
	End Sub

	' Token: 0x0600338C RID: 13196 RVA: 0x001DF57F File Offset: 0x001DD97F
	Public Sub EnableCollider(enable As Boolean)
		If Me.collider2d IsNot Nothing Then
			Me.collider2d.enabled = enable
		End If
	End Sub

	' Token: 0x0600338D RID: 13197 RVA: 0x001DF59E File Offset: 0x001DD99E
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		If Me.OnDestroyCallback IsNot Nothing Then
			Me.OnDestroyCallback(Me)
		End If
		Me.spark = Nothing
	End Sub

	' Token: 0x04003BD9 RID: 15321
	Private Const KetchupState As String = "Ketchup"

	' Token: 0x04003BDA RID: 15322
	Private Const MustardState As String = "Mustard"

	' Token: 0x04003BDB RID: 15323
	Private Const RelishState As String = "Relish"

	' Token: 0x04003BDC RID: 15324
	Private Const Ketchup As String = "K"

	' Token: 0x04003BDD RID: 15325
	Private Const Mustard As String = "M"

	' Token: 0x04003BDE RID: 15326
	Private Const Relish As String = "R"

	' Token: 0x04003BDF RID: 15327
	<SerializeField()>
	Private scaleFactor As Single

	' Token: 0x04003BE0 RID: 15328
	<SerializeField()>
	Private renderers As SpriteRenderer()

	' Token: 0x04003BE1 RID: 15329
	<SerializeField()>
	Private spark As Effect

	' Token: 0x04003BE2 RID: 15330
	Private collider2d As Collider2D
End Class
