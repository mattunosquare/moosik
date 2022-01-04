import React, { useState } from "react";
import { Card } from "@mui/material";
import CardActions from "@mui/material/CardActions";
import CardContent from "@mui/material/CardContent";
import Button from "@mui/material/Button";
import Grid from "@mui/material/Grid";
import Modal from "@mui/material/Modal";
import ReplyDetailsModal from "./reply-details-modal";
import Typography from "@mui/material/Typography";
import { IPostDetails, IThreadDetails } from "../../../Common/Interfaces";

interface IProps {
  post: IPostDetails;
  thread: IThreadDetails;
}

const PostReplyCard = ({ post, thread }: IProps) => {
  const [viewModalOpen, setViewModalOpen] = useState(false);
  const handleOpenViewModal = () =>
    setViewModalOpen((current: boolean) => !current);
  const handleCloseViewModal = () => setViewModalOpen(false);
  console.log(post);
  return (
    <>
      <Grid container justifyContent="flex-end" sx={{ width: "100vw" }}>
        <Grid item sx={{ width: "85vw" }}>
          <Card>
            <CardContent>{post.description}</CardContent>
            <CardActions>
              <Grid container justifyContent="space-between">
                <CardContent>
                  <Typography variant="body2">
                    by {post.user.username} (ID:{post.user.id}) on{" "}
                    {post.createdDate}
                  </Typography>
                </CardContent>
                <Button
                  size="small"
                  onClick={() => {
                    handleOpenViewModal();
                  }}
                >
                  view
                </Button>
              </Grid>
            </CardActions>
          </Card>
        </Grid>
      </Grid>

      <Modal
        open={viewModalOpen}
        onClose={handleCloseViewModal}
        aria-labelledby="modal-modal-title"
        aria-describedby="modal-modal-description"
      >
        <ReplyDetailsModal post={post} thread={thread} />
      </Modal>
    </>
  );
};

export default PostReplyCard;
