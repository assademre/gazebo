import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { useParams, useNavigate, Link } from 'react-router-dom';
import { getTaskByTaskIdAPI, addCommentAPI, getCommentsAPI } from '../../api';
import statusOptions from '../../helpers/statusOptions';
import { format } from 'date-fns';
import "./Task.css";
import Layout from '../../NavigationBar/Layout';

function Task() {
  const { t } = useTranslation();
  const { taskId } = useParams();
  const [task, setTask] = useState(null);
  const [comments, setComments] = useState([]);
  const [newComment, setNewComment] = useState('');
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize] = useState(10);
  const [totalPages, setTotalPages] = useState(1);
  const navigate = useNavigate();

  useEffect(() => {
    fetchData();
  }, [pageNumber, pageSize]);

  const fetchData = async () => {
    try {
      const taskData = await getTaskByTaskIdAPI(taskId);
      setTask(taskData);
      const { comments: commentsData, totalPages } = await getCommentsAPI(2, taskId, pageNumber, pageSize);
      setComments(commentsData || []);
      setTotalPages(totalPages);
    } catch (error) {
      console.error('Error fetching task data:', error);
      navigate('/get-tasks');
    }
  };

  const getStatusLabel = (statusValue) => {
    const statusOption = statusOptions.find(option => option.value === statusValue);
    return statusOption ? statusOption.label : 'Unknown';
  };

  const handleCommentChange = (e) => {
    setNewComment(e.target.value);
  };

  const handleAddComment = async () => {
    if (newComment.trim() !== '') {
      try {
        const userId = localStorage.getItem('userId');
        const commentData = {
          postGroupTypeId: 2,
          postGroupId: taskId,
          commentOwnerId: userId,
          commentText: newComment,
          commentDate: new Date().toISOString()
        };
        await addCommentAPI(commentData);
        fetchData();
        setNewComment('');
      } catch (error) {
        console.error('Error adding comment:', error);
      }
    }
  };

  const handleKeyPress = (e) => {
    if (e.key === 'Enter' && !e.shiftKey) {
      e.preventDefault();
      handleAddComment();
    }
  };

  const handlePreviousPage = () => {
    setPageNumber(prevPageNumber => Math.max(prevPageNumber - 1, 1));
  };

  const handleNextPage = () => {
    setPageNumber(prevPageNumber => Math.min(prevPageNumber + 1, totalPages));
  };

  if (!task) {
    return <div>{t('loading')}</div>;
  }

  const formattedDate = format(new Date(task.taskDate), 'dd-MM-yyyy');

  return (
    <Layout>
      <div className="task-page-container">
        <div className="task-details">
          <div className="task-detail-item">
            <strong>{t('taskName')}:</strong> <span>{task.taskName}</span>
          </div>
          <div className="task-detail-item">
            <strong>{t('eventName')}:</strong>
            <span>
              <Link to={`/event/${task.eventId}`} style={{ color: 'black' }}>{task.eventName}</Link>
            </span>
          </div>
          <div className="task-detail-item">
            <strong>{t('taskDate')}:</strong> <span>{formattedDate}</span>
          </div>
          <div className="task-detail-item">
            <strong>{t('status')}:</strong> <span>{t(getStatusLabel(task.status))}</span>
          </div>
          <div className="button-container">
            <Link to={`/edit-task/${taskId}`}>
              <button>{t('edit')}</button>
            </Link>
            <button onClick={() => navigate(-1)}>{t('cancel')}</button>
          </div>
        </div>
        <div className="comments-section">
          <h3>{t('comments')}</h3>
          <div className="comments-list">
            {comments.map((comment) => (
              <div key={comment.commentId} className="comment-item">
                <p>
                  <strong>
                    <Link to={`/profile/${comment.commentOwnerId}`} style={{ color: 'black' }}>
                      {comment.commentOwnerName}
                    </Link>
                  </strong> {format(new Date(comment.commentDate), 'dd-MM-yyyy HH:mm')}
                </p>
                <p>{comment.commentText}</p>
              </div>
            ))}
          </div>
          <div className="pagination">
            <button onClick={handlePreviousPage} disabled={pageNumber === 1}>&lt;</button>
            <span>{t('page')} {pageNumber} {t('of')} {totalPages}</span>
            <button onClick={handleNextPage} disabled={pageNumber === totalPages}>&gt;</button>
          </div>
          <div className="add-comment">
            <textarea 
              value={newComment}
              onChange={handleCommentChange}
              onKeyPress={handleKeyPress}
              placeholder={t('addYourComment')}
            />
            <button onClick={handleAddComment}>{t('addComment')}</button>
          </div>
        </div>
      </div>
    </Layout>
  );
}

export default Task;
