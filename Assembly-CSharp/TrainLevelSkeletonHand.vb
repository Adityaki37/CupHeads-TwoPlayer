Imports System
Imports UnityEngine

' Token: 0x0200082B RID: 2091
Public Class TrainLevelSkeletonHand
	Inherits AbstractTrainLevelSkeletonPart

	' Token: 0x06003094 RID: 12436 RVA: 0x001C9890 File Offset: 0x001C7C90
	Private Sub PlaySlapFX()
		Me.effectPrefab.Create(Me.effectRoot.position, MyBase.transform.localScale)
		CupheadLevelCamera.Current.Shake(20F, 0.6F, False)
	End Sub

	' Token: 0x06003095 RID: 12437 RVA: 0x001C98C9 File Offset: 0x001C7CC9
	Public Sub Slap()
		MyBase.animator.Play("Slap")
	End Sub

	' Token: 0x06003096 RID: 12438 RVA: 0x001C98DB File Offset: 0x001C7CDB
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.effectPrefab = Nothing
	End Sub

	' Token: 0x04003935 RID: 14645
	<SerializeField()>
	Private effectRoot As Transform

	' Token: 0x04003936 RID: 14646
	<SerializeField()>
	Private effectPrefab As Effect
End Class
