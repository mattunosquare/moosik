import React, { useState } from "react";
import { Card } from "@mui/material";
import CardActions from "@mui/material/CardActions";
import CardContent from "@mui/material/CardContent";
import Button from "@mui/material/Button";
import Typography from "@mui/material/Typography";
import Grid from "@mui/material/Grid";
import Modal from "@mui/material/Modal";
import ReplyModal from "./reply-modal";
import PostDetailsModal from "./post-details-modal";
import { IThreadDetails } from "../../../Common/Interfaces";

interface IProps {
  thread: IThreadDetails;
}

const PostCard = ({ thread }: IProps) => {
  const [replyModalOpen, setReplyModalOpen] = useState(false);
  const toggleReplyModal = (current: boolean) => setReplyModalOpen(!current);

  const [viewModalOpen, setViewModalOpen] = useState(false);
  const toggleViewModal = (current: boolean) => setViewModalOpen(!current);

  return (
    <>
      <Grid
        container
        justifyContent="flex-end"
        sx={{
          width: "100vw",
        }}
      >
        <Grid item sx={{ width: "95vw" }}>
          <Card variant="outlined">
            <CardContent>
              <Typography variant="subtitle2">
                {thread.threadType.description}
              </Typography>
              <Typography variant="h6">{thread.title}</Typography>

              <Typography variant="subtitle1">
                {thread.posts[0].description}
              </Typography>
            </CardContent>
            <Grid container justifyContent="space-between">
              <CardContent>
                <Typography variant="body2">
                  by {thread.user.username} on {thread.createdDate}
                </Typography>
              </CardContent>
              <CardActions>
                <Button
                  size="small"
                  onClick={() => {
                    toggleReplyModal(replyModalOpen);
                  }}
                >
                  Reply
                </Button>
                <Button
                  size="small"
                  onClick={() => {
                    toggleViewModal(viewModalOpen);
                  }}
                >
                  view
                </Button>
              </CardActions>
            </Grid>
          </Card>
        </Grid>
      </Grid>

      <Modal
        open={replyModalOpen}
        onClose={() => {
          toggleReplyModal(replyModalOpen);
        }}
        aria-labelledby="modal-modal-title"
        aria-describedby="modal-modal-description"
      >
        <ReplyModal thread={thread} />
      </Modal>

      <Modal
        open={viewModalOpen}
        onClose={() => {
          toggleViewModal(viewModalOpen);
        }}
        aria-labelledby="modal-modal-title"
        aria-describedby="modal-modal-description"
      >
        <PostDetailsModal thread={thread} />
      </Modal>
    </>
  );
};

export default PostCard;
