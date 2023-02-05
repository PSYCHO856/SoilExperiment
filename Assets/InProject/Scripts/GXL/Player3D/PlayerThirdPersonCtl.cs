using System.Net.Mime;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/// <summary>
/// 新的第三人称角色动画控制器角色1
/// </summary>
public class PlayerThirdPersonCtl : MonoBehaviour {
	public bool m_IsGrounded=true;
	public float m_GroundCheckDistance = 0.1f;//着陆范围
	public Animator m_Animator;
	public GameObject audioObj;
	private CharacterController cc;
	// Use this for initialization
	void Start () {

		cc= GetComponentInChildren<CharacterController>();
		if(!m_Animator){
			m_Animator = GetComponent<Animator>();
		}
	}
	 private void LateUpdate(){
			//待机
		if (cc.isGrounded && ETCInput.GetAxis("Vertical")==0 && ETCInput.GetAxis("Horizontal")==0){
			m_Animator.SetFloat("Forward",0);
			m_Animator.SetFloat("Turn", 0);
		}
		//前跑
		if (cc.isGrounded && (ETCInput.GetAxis("Vertical")>0)){
			m_Animator.SetFloat("Forward", 1);
			m_Animator.SetFloat("Turn", 0);
		}
		//后跑
		if (cc.isGrounded &&(ETCInput.GetAxis("Vertical")<0)){
			m_Animator.SetFloat("Forward", -1);
			m_Animator.SetFloat("Turn", 0);
		}
		if (ETCInput.GetAxis("Vertical")==0 && ETCInput.GetAxis("Horizontal")>0){
			m_Animator.SetFloat("Forward", 0);
			m_Animator.SetFloat("Turn", 1);
		}

		if ( ETCInput.GetAxis("Vertical")==0 && ETCInput.GetAxis("Horizontal")<0){
			m_Animator.SetFloat("Forward", 0);
			m_Animator.SetFloat("Turn", -1);
		}
		if (!cc.isGrounded){
			// m_Animator.SetFloat("Turn", -1, 0.1f, Time.deltaTime);
		}
		if(Input.GetAxis("Vertical")!=0){
			audioObj.SetActive(true);
		}else{
			audioObj.SetActive(false);
		}
		CheckGroundStatus();
	}
	Vector3 m_GroundNormal;
	void CheckGroundStatus()
	{
		RaycastHit hitInfo;
		Debug.DrawLine(transform.position , transform.position+ (Vector3.down * m_GroundCheckDistance),Color.red);
		if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, m_GroundCheckDistance))
		{
			m_GroundNormal = hitInfo.normal;
			m_IsGrounded = true;
			m_Animator.SetBool("OnGround",true);
		}
		else
		{
			m_IsGrounded = false;
			m_GroundNormal = Vector3.up;
			m_Animator.SetBool("OnGround",false);
			
			m_Animator.SetFloat("Jump", -5f);
			m_Animator.SetFloat("Jump", 5f, 1f, Time.deltaTime*2);
		}
	}
}
