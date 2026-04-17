Imports System
Imports UnityEngine

' Token: 0x02000A44 RID: 2628
Public Class CharmFloatWings
	Inherits MonoBehaviour

	' Token: 0x06003EAB RID: 16043 RVA: 0x00225D5C File Offset: 0x0022415C
	Private Sub Start()
		If Me.useWindEffect Then
			Dim componentsInChildren As SpriteRenderer() = MyBase.GetComponentsInChildren(Of SpriteRenderer)()
			For Each spriteRenderer As SpriteRenderer In componentsInChildren
				spriteRenderer.enabled = False
			Next
		End If
	End Sub

	' Token: 0x06003EAC RID: 16044 RVA: 0x00225D9C File Offset: 0x0022419C
	Private Sub OnEnable()
		If Not Me.useWindEffect Then
			Me.SpawnFeathers()
		End If
	End Sub

	' Token: 0x06003EAD RID: 16045 RVA: 0x00225DAF File Offset: 0x002241AF
	Private Sub OnDisable()
		If Not Me.useWindEffect Then
			Me.SpawnFeathers()
		End If
	End Sub

	' Token: 0x06003EAE RID: 16046 RVA: 0x00225DC4 File Offset: 0x002241C4
	Private Sub SpawnFeathers()
		For i As Integer = 0 To 10 - 1
			Me.feather.Create(MyBase.transform.position, New Vector3(Global.UnityEngine.Random.Range(0.5F, 1F), Global.UnityEngine.Random.Range(0.5F, 1F)))
		Next
	End Sub

	' Token: 0x06003EAF RID: 16047 RVA: 0x00225E20 File Offset: 0x00224220
	Private Sub Update()
		If Me.useWindEffect Then
			Me.spawnTime += CupheadTime.Delta
			If Me.spawnTime > Me.spawnFreq Then
				Me.spawnTime -= Me.spawnFreq
				Me.featherAlt.Create(MyBase.transform.parent.position + Vector3.down * 10F)
			End If
		End If
	End Sub

	' Token: 0x040045B3 RID: 17843
	<SerializeField()>
	Private feather As Effect

	' Token: 0x040045B4 RID: 17844
	<SerializeField()>
	Private featherAlt As Effect

	' Token: 0x040045B5 RID: 17845
	<SerializeField()>
	Private useWindEffect As Boolean

	' Token: 0x040045B6 RID: 17846
	Private spawnFreq As Single = 0.1F

	' Token: 0x040045B7 RID: 17847
	Private spawnTime As Single
End Class
