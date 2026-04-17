Imports System
Imports UnityEngine

' Token: 0x020007D9 RID: 2009
Public Class SaltbakerLevelTable
	Inherits MonoBehaviour

	' Token: 0x06002DE0 RID: 11744 RVA: 0x001B0E4A File Offset: 0x001AF24A
	Private Sub Start()
		MyBase.GetComponent(Of MeshRenderer)().sortingOrder = -1
		Me.tableMesh = Me.tableMeshFilter.mesh
		Me.vertices = Me.tableMesh.vertices
		Me.cam = CupheadLevelCamera.Current
	End Sub

	' Token: 0x06002DE1 RID: 11745 RVA: 0x001B0E88 File Offset: 0x001AF288
	Private Sub Update()
		Dim num As Single = Mathf.InverseLerp(Me.cam.Right, Me.cam.Left, Me.cam.transform.position.x) - 0.5F
		Me.vertices(0).x = -0.5F + num * Me.skewFactor
		Me.vertices(2).x = 0.5F + num * Me.skewFactor
		Me.tableMesh.vertices = Me.vertices
		Me.tableMesh.RecalculateBounds()
	End Sub

	' Token: 0x04003662 RID: 13922
	<SerializeField()>
	Private skewFactor As Single = 0.02F

	' Token: 0x04003663 RID: 13923
	<SerializeField()>
	Private tableMeshFilter As MeshFilter

	' Token: 0x04003664 RID: 13924
	Private tableMesh As Mesh

	' Token: 0x04003665 RID: 13925
	Private vertices As Vector3()

	' Token: 0x04003666 RID: 13926
	Private cam As CupheadLevelCamera
End Class
