import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { useParams, useNavigate, Link } from 'react-router-dom';
import { getEventByEventIdAPI, addCommentAPI, getCommentsAPI } from '../../api';
import statusOptions from '../../helpers/statusOptions';
import { format } from 'date-fns';
import "./Event.css";
import Layout from '../../NavigationBar/Layout';

function Event() {
  const { t } = useTranslation();
  const { eventId } = useParams();
  const [event, setEvent] = useState(null);
  const [comments, setComments] = useState([]);
  const [newComment, setNewComment] = useState('');
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize] = useState(10);
  const [totalPages, setTotalPages] = useState(0);
  const navigate = useNavigate();

  useEffect(() => {
    fetchData();
  }, [pageNumber]);

  const fetchData = async () => {
    try {
      const eventData = await getEventByEventIdAPI(eventId);
      setEvent(eventData);
      const { comments: commentsData, totalPages: totalPagesData } = await getCommentsAPI(1, eventId, pageNumber, pageSize);
      setComments(commentsData || []);
      setTotalPages(totalPagesData);
    } catch (error) {
      console.error('Error fetching event data:', error);
      navigate('/get-events');
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
          postGroupTypeId: 1,
          postGroupId: eventId,
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
    if (pageNumber > 1) {
      setPageNumber(pageNumber - 1);
    }
  };

  const handleNextPage = () => {
    if (pageNumber < totalPages) {
      setPageNumber(pageNumber + 1);
    }
  };

  if (!event) {
    return <div>{t('loading')}</div>;
  }

  const formattedDate = format(new Date(event.eventDate), 'dd-MM-yyyy');

  return (
    <Layout>
      <div className="event-page-wrapper">
        <div className="event-page-container">
          <div className="event-details">
            <div className="event-detail-item">
              <strong>{t('eventName')}:</strong> <span>{event.eventName}</span>
            </div>
            <div className="event-detail-item">
              <strong>{t('budget')}:</strong> <span>{event.budget}</span>
            </div>
            <div className="event-detail-item">
              <strong>{t('place')}:</strong> <span>{event.place}</span>
            </div>
            <div className="event-detail-item">
              <strong>{t('eventDate')}:</strong> <span>{formattedDate}</span>
            </div>
            <div className="event-detail-item">
              <strong>{t('status')}:</strong> <span>{t(getStatusLabel(event.status))}</span>
            </div>
            <div className="button-container">
              <Link to={`/edit-event/${eventId}`}>
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
                    <Link to={`/profile/${comment.commentOwnerId}`} style={{ color: 'black' }}>
                      <strong>{comment.commentOwnerName}</strong>
                    </Link>
                    {' '}
                    {format(new Date(comment.commentDate), 'dd-MM-yyyy HH:mm')}
                  </p>
                  <p>{comment.commentText}</p>
                </div>
              ))}
            </div>
            <div className="pagination">
              <button onClick={handlePreviousPage} disabled={pageNumber === 1}>&lt;</button>
              <span>{`${t('page')} ${pageNumber} ${t('of')} ${totalPages}`}</span>
              <button onClick={handleNextPage} disabled={pageNumber >= totalPages}>&gt;</button>
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
      </div>
    </Layout>
  );
}

export default Event;
