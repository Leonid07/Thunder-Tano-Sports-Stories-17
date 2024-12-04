using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speed = 5f; // �������� �����������
    public float smoothTime = 0.3f; // ����� �����������
    public float rotationSpeed = 5f; // �������� ��������

    public List<Transform> points = new List<Transform>(); // ������ �����
    public int currentPointIndex = 0; // ������ ������� �����
    public bool isWaitingForButtonPress = false; // ���� ��� ��������, ��������� �� ������� ������
    private Coroutine moveCoroutine;

    public void StartMovetment()
    {
        moveCoroutine = StartCoroutine(MoveToPoints());
    }

    private IEnumerator MoveToPoints()
    {
        while (true)
        {
            if (points.Count == 0)
            {
                //continueButton.gameObject.SetActive(false);
                yield break;
            }

            // ��������� � ��������� �����
            Vector3 startPosition = transform.position;
            Vector3 targetPosition = points[currentPointIndex].position;
            Quaternion startRotation = transform.rotation;
            Quaternion targetRotation = Quaternion.LookRotation((targetPosition - startPosition).normalized);

            float journeyLength = Vector3.Distance(startPosition, targetPosition);
            float startTime = Time.time;

            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                float distanceCovered = (Time.time - startTime) * speed;
                float fractionOfJourney = distanceCovered / journeyLength;

                transform.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);
                transform.rotation = Quaternion.Slerp(startRotation, targetRotation, fractionOfJourney * rotationSpeed);

                yield return null;
            }

            // ������������, ����� ��������� ������� � ������� ����� ��������������� �������
            transform.position = targetPosition;
            transform.rotation = targetRotation;

            // �������� �� ������� ���������� StopPoint � ����� OnReached
            StopPoint stopPoint = points[currentPointIndex].GetComponent<StopPoint>();
            if (stopPoint != null)
            {
                stopPoint.ActiveCanvas();
                if (stopPoint.requiresPause)
                {
                    // �������� �� ��������� ����� ����� ���������� ������
                    if (currentPointIndex >= points.Count - 1)
                    {
                        //continueButton.gameObject.SetActive(false);
                        yield break; // ��������� ��������, ���� �������� ��������� �����
                    }

                    // ���������� ������ � ������������� ���� ��������
                    //continueButton.gameObject.SetActive(true);
                    isWaitingForButtonPress = true;

                    // �������� ������� ������ ��� �����������
                    yield return new WaitUntil(() => !isWaitingForButtonPress);
                }
            }

            // ���� �������� ��������� �����, ��������� ��������
            if (currentPointIndex >= points.Count - 1)
            {
                Debug.Log("���������� ��������� �����");
                //continueButton.gameObject.SetActive(false);
                yield break; // ��������� ��������, ���� �������� ��������� �����
            }

            currentPointIndex++;
        }
    }

    public void OnContinueButtonPressed()
    {
        // �������� ������ � ���������� ���� ��������
        isWaitingForButtonPress = false;

        // ���������, ��������� �� �������� �� ��������� �����
        if (currentPointIndex >= points.Count - 1)
        {
            isWaitingForButtonPress = true;
        }
    }
    public void RestartMovement()
    {
        // ������������� ������� ��������, ���� ��� ��������
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        // ���������� ������ ������� ����� � ������������� ��������
        currentPointIndex = 0;
        transform.position = points[0].position;
        transform.Rotate(0, 0, 0);
        gameObject.SetActive(true);
        moveCoroutine = StartCoroutine(MoveToPoints());
    }
}