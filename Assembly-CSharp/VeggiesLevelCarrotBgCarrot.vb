Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000847 RID: 2119
Public Class VeggiesLevelCarrotBgCarrot
	Inherits AbstractMonoBehaviour

	' Token: 0x0600310A RID: 12554 RVA: 0x001CC940 File Offset: 0x001CAD40
	Public Function Create(side As Integer, speed As Single, parentCarrot As VeggiesLevelCarrot) As VeggiesLevelCarrotBgCarrot
		Dim veggiesLevelCarrotBgCarrot As VeggiesLevelCarrotBgCarrot = Me.InstantiatePrefab(Of VeggiesLevelCarrotBgCarrot)()
		veggiesLevelCarrotBgCarrot.Init(side, speed, parentCarrot)
		Return veggiesLevelCarrotBgCarrot
	End Function

	' Token: 0x0600310B RID: 12555 RVA: 0x001CC960 File Offset: 0x001CAD60
	Private Sub Init(side As Integer, speed As Single, parentCarrot As VeggiesLevelCarrot)
		MyBase.transform.SetPosition(New Single?(Global.UnityEngine.Random.Range(150F, 600F) * CSng(side)), New Single?(-360F), New Single?(0F))
		Me.parentCarrot = parentCarrot
		MyBase.StartCoroutine(Me.float_cr(speed))
	End Sub

	' Token: 0x0600310C RID: 12556 RVA: 0x001CC9B8 File Offset: 0x001CADB8
	Private Sub [End]()
		Me.StopAllCoroutines()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x0600310D RID: 12557 RVA: 0x001CC9CC File Offset: 0x001CADCC
	Private Iterator Function float_cr(speed As Single) As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While True
			If MyBase.transform.position.y > 720F Then
				Me.parentCarrot.ShootHoming()
				Me.[End]()
			End If
			MyBase.transform.AddPosition(0F, speed * CupheadTime.FixedDelta, 0F)
			Yield wait
		End While
		Return
	End Function

	' Token: 0x040039AF RID: 14767
	Private Const RANGE_MIN As Single = 150F

	' Token: 0x040039B0 RID: 14768
	Private Const RANGE_MAX As Single = 600F

	' Token: 0x040039B1 RID: 14769
	Private parentCarrot As VeggiesLevelCarrot
End Class
